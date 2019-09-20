import { notification } from 'antd';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
export default {
	namespace: 'renyua',
	state: {
		data: [{
			id: 1,
			title: '第一题',
			answers:["A", "B", "C","D"]
		}, {
			id: 2,
			title: '第二题',
			answers:["A", "B", "C","D"]
		}, {
			id: 3,
			title: '第三题',
			answers:["A", "B", "C","D"]
		}, {
			id: 4,
			title: '第四题',
			answers:["A", "B", "C","D"]
		}],
		isLoading: false,

		values: {},
	},
	//订阅
	subscriptions: {
		setup({ dispatch, history }) {

		}
	},

	effects: {
		*createrenyua({ payload }, { call, put }) {
			const data = yield call(
				...createApiAuthParam({
					method: new api.RenyuaApi().appRenyuaCreate,
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
