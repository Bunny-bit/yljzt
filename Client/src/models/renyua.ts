import { notification } from 'antd';
import { routerRedux } from 'dva/router';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
export default {
	namespace: 'renyua',
	state: {
		timus: [],
		isLoading: false,
		answers: {},
		xueyuan: [{
			label: '酒店管理学院',
			value: '酒店管理学院'
		},{
			label: '旅游管理学院',
			value: '旅游管理学院'
		},{
			label: '经济管理学院',
			value: '经济管理学院'
		},{
			label: '外语学院',
			value: '外语学院'
		},{
			label: '信息技术与传媒学院',
			value: '信息技术与传媒学院'
		},{
			label: '资源工程学院',
			value: '资源工程学院'
		},{
			label: '文化艺术学院',
			value: '文化艺术学院'
		},{
			label: '互联网学院',
			value: '互联网学院'
		},{
			label: '基础教学部',
			value: '基础教学部'
		},{
			label: '思想政治教学部',
			value: '思想政治教学部'
		},{
			label: '体育教学部',
			value: '体育教学部'
		},{
			label: '继续教育中心',
			value: '继续教育中心'
		}]
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
							maxResultCount: 10
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
				yield put(routerRedux.push('/tankuang'));
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
				data.result.items.map(item => {
					let answers = [];
					for (var i in item.xuanxiangs) {
						let xx = item.xuanxiangs[i]
						answers.push({
							xuanxiangId: xx.id,
							timuId: xx.timuId,
							name: xx.name,
							neirong: xx.neirong
						});
					}
					timus.push({
						id: item.id,
						title: item.tiHao + ". " + item.tiMu,
						answers: answers
					})
				})
				yield put({
					type: "setState",
					payload: { timus: timus }
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
