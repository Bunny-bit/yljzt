/**
 * apiUtil.ts
 * Created by 李廷旭 on 2017/10/13 9:55
 * 描述: dva 整合TypeScript接口工具调用封装
 */
import * as api from './../api/api';
import { notification } from 'antd';

export interface Parm {
	method: Function;
	payload?: Object;
}

function callApi(method, params, options): any {
	var result = method.apply(
		this,
		method.length == 2
			? [ params, options ]
			: method.length == 3 ? [ '', '', options ] : [ params, '', '', options ]
	);
	return result
		.then((response) => {
			if (response.json) {
				return response.json();
			}
			return response;
		})
		.catch(function(error) {
			//所有接口的异常除了200-300之间的状态码
			// console.log(error);
			if (error.status === 500) {
				error.text().then((text) => {
					// console.log(text);
					let data = {};
					try {
						data = JSON.parse(text);
					} catch (e) {
						console.log(e);
						notification.error({
							message: '系统异常',
							description: '接口返回数据不是json对象！'
						});
					}
					notification.error({
						message: data.error.message,
						description:
							data.error.details && data.error.details.length > 200
								? data.error.details.substring(0, 200)
								: data.error.details
					});
				});
				// 500状态给返回
				return error;
			}
			// 除了500的抛出给index处理
			throw error;
		});
}

export function createApiAuthParam(parm: Parm): Array<any> {
	let token = localStorage.getItem('token');
	let payload = parm.payload ? parm.payload : {};
	let param = {
		...payload, //get参数
		input: payload //post参数
		// authorization: 'Bearer ', //authorization在param里面
	};
	let options = {
		withCredentials: true,
		credentials: 'include',
		headers: {
			Authorization: `Bearer ${token}`
		}
	};
	return [ [ api, callApi ], parm.method, param, options ];
}
