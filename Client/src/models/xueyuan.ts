
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';


import { notification } from 'antd';
export default {
	namespace: 'xueyuan',
	state: {

	},
	subscriptions: {
		setup({ dispatch, history }) { 
			
		}
	},

	effects: {
		*creatXueyuan({ payload }, { call, put }) {
			const data = yield call(
				...createApiAuthParam({
					method: new api.XueyuanApi().appXueyuanCreate,
					payload: payload
				})
			);
			if (data.success) {
				notification.success({
					message: '成功',
					description: '恭喜你登录成功'
				});
			}
		},


		reducers: {
			setState(state, { payload }) {
				return {
					...state,
					...payload
				};
			}
		}
	}
}
