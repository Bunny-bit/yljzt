import moment from 'moment';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

export default {
	namespace: 'appEdition',
	state: {
		modalVisible: false,
		items: [],
		record: {},
		modalText: '',
		isAdd: true,
		isIOS: true,
		//分页信息
		pagination: {
			total: 0,
			current: 1,
			pageSize: 10,
			showSizeChanger: true,
			showQuickJumper: true,
			showTotal: (total) => `共 ${total} 条`,
			size: 'large'
		},
		search: {},
		filters: {},
		sorter: {}
	},
	reducers: {
		setState(state, { payload }) {
			return {
				...state,
				...payload
			};
		},
		setPages(state, { payload }) {
			return {
				...state,
				items: payload.items,
				pagination: {
					...state.pagination,
					...payload.pagination
				}
			};
		}
	},
	effects: {
		*getAppEditions({ payload }, { call, put, select }) {
			var state = yield select(({ appEdition }) => appEdition);
			payload = { ...state.pagination, ...payload };
			var body = {
				filter: payload.filter,
				sorting: payload.field ? payload.field + ' ' + payload.order.replace('end', '') : '',
				maxResultCount: payload.pageSize,
				skipCount: payload.pageSize * (payload.current - 1)
			};
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.AppEditionsApi().appAppEditionsGetAppEditions,
					payload: body
				})
			);
			if (success) {
				yield put({
					type: 'setPages',
					payload: {
						items: result.items,
						pagination: {
							...payload,
							total: result.totalCount,
							current: payload.current,
							pageSize: payload.pageSize
						}
					}
				});
			}
		},
		*createOrUpdate({ payload }, { call, put, select }) {
			let state = yield select(({ appEdition }) => appEdition);
			if (payload.itunesUrl === '') {
				payload.itunesUrl = null;
			}
			let method;
			if (state.isAdd) {
				if (state.isIOS) {
					method = new api.AppEditionsApi().appAppEditionsCreateIOSAppEdition;
				} else {
					method = new api.AppEditionsApi().appAppEditionsCreateAndroidAppEdition;
				}
			} else {
				if (state.isIOS) {
					method = new api.AppEditionsApi().appAppEditionsUpdateIOSAppEdition;
				} else {
					method = new api.AppEditionsApi().appAppEditionsUpdateAndroidAppEdition;
				}
			}
			const { success, result } = yield call(
				...createApiAuthParam({
					method: method,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						modalVisible: false
					}
				});
				yield put({
					type: 'getAppEditions'
				});
			}
		},
		*deleteAppEdition({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.AppEditionsApi().appAppEditionsDeleteAppEdition,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'getAppEditions'
				});
			}
		}
	},
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				if (pathname.toLowerCase() == '/appEdition'.toLowerCase()) {
					dispatch({
						type: 'getAppEditions',
						payload: {
							current: 1,
							pageSize: 10
						}
					});
				}
			});
		}
	}
};
