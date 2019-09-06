import React from 'react';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
import { message } from 'antd';

export default {
	namespace: 'getSet',
	state: {
		data: {}
	},
	reducers: {
		setState(state, { payload }) {
			return {
				...state,
				...payload
			};
		}
	},
	effects: {
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
						data: result
					}
				});
			}
		},
		*set({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.api,
					payload: payload.data
				})
			);
			if (success) {
				message.success('保存成功');
			}
		}
	},
	subscriptions: {
	}
};
