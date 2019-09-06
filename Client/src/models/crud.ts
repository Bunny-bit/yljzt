import { createApiAuthParam } from './../api/apiUtil.js';

export default {
	namespace: 'crud',
	state: {
		modalVisible: false,
		items: [],
		record: {},
		modalText: '',
		isAdd: true,
		pagination: {
			total: 0,
			current: 1,
			pageSize: 10,
			showSizeChanger: true,
			showQuickJumper: true,
			showTotal: (total) => `共 ${total} 条`,
			size: 'large'
		},
		filters: {},
		sorter: {},
		selectedIds: [],
		selectedRows: []
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
				},
				filters: payload.filters,
				sorter: payload.sorter
			};
		}
	},
	effects: {
		*getAll({ payload }, { call, put, select }) {
			var state = yield select(({ crud }) => crud);
			var pagination = {
				...state.pagination,
				...payload && payload.data && payload.data.pagination
			};
			var filters = {
				...state.filters
			};
			if (payload && payload.data && payload.data.filters) {
				filters = payload.data.filters;
			}
			var sorter = {
				...state.sorter,
				...payload && payload.data && payload.data.sorter
			};
			if (payload && payload.data && payload.data.clear) {
				pagination = {
					current: 1,
					pageSize: 10
				};
				filters = {};
				sorter = {};
			}
			var body = {
				sorting: sorter.field ? sorter.field.replace(/^_/, '') + ' ' + sorter.order.replace('end', '') : '',
				maxResultCount: pagination.pageSize,
				skipCount: pagination.pageSize * (pagination.current - 1),
				...filters
			};
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
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
							current: pagination.current,
							pageSize: pagination.pageSize
						},
						filters: filters,
						sorter: sorter,
						record: {}
					}
				});
			}
		},
		*create({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
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
					type: 'getAll',
					payload: {
						api: payload.getAllApi
					}
				});
			}
		},
		*update({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
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
					type: 'getAll',
					payload: {
						api: payload.getAllApi
					}
				});
			}
		},
		*delete({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
				})
			);
			if (success) {
				yield put({
					type: 'getAll',
					payload: {
						api: payload.getAllApi
					}
				});
			}
		},
		*deleteBatch({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						selectedIds: [],
						selectedRows: []
					}
				});
				yield put({
					type: 'getAll',
					payload: {
						api: payload.getAllApi
					}
				});
			}
		},
		*get({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						record: { id: result.id },
						modalVisible: true,
						modalText: '编辑',
						isAdd: false
					}
				});
				for (var key in result) {
					payload.form.getFieldDecorator(key);
				}
				payload.form.setFieldsValue(result);
			}
		},
		*toExcel({ payload }, { call, put, select }) {
			var state = yield select(({ crud }) => crud);
			var filters = {
				...state.filters,
				...payload && payload.data && payload.data.filters
			};
			var sorter = {
				...state.sorter,
				...payload && payload.data && payload.data.sorter
			};
			var body = {
				sorting: sorter.field ? sorter.field.replace(/^_/, '') + ' ' + sorter.order.replace('end', '') : '',
				...filters
			};
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: body
				})
			);
			if (success) {
				yield put({
					type: 'download/downloadTempFile',
					payload: {
						...result
					}
				});
			}
		}
	},
	subscriptions: {}
};
