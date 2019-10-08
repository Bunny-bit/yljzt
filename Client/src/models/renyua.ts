import { notification } from 'antd';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
export default {
	namespace: 'renyua',
	state: {
		timus: [],
		isLoading: false,
		answers: {}
	},
	//订阅
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				if (pathname.toLowerCase() == '/renyuas'.toLowerCase()) {
					dispatch({
						type: 'getTimus',
						payload: {
							current: 1,
							maxResultCount: 1
						}
					});
				}
			});
		}
	},

	effects: {
		*submit({ payload }, { call, put }) {
			const data = yield call(
				...createApiAuthParam({
					method: new api.DaanApi().appDaanAnswer,
					payload: payload
				})
			);
			if (data.success) {
				notification.success({
					message: '保存成功',
					description: '恭喜你保存成功'
				});
			}
		},
		*getTimus({ payload }, { call, put }) {
			const data = yield call(
				...createApiAuthParam({
					method: new api.YljztApi().appYljztGetDajuan,
					payload: payload
				})
			);
			if (data.success) {
				let timus = [];
				data.result.items.map(item=>{
					let answers = [];
					for(var i in item.xuanxiangs){
						let xx = item.xuanxiangs[i]
						answers.push({
							xuanxiangId:xx.id, 
							timuId: xx.timuId, 
							name:xx.name, 
							neirong:xx.neirong
						});
					}
					timus.push({
						id:item.id,
						title:item.tiHao + ". " + item.tiMu,
						answers:answers
					})
				})
				yield put({
					type:"setState",
					payload:{timus:timus}
				})
			}
		},
	},

	reducers: {
		setState(state, { payload }) {
			return {
				...state,
				...payload
			};
		}
	}
};
