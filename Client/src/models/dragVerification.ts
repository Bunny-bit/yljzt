import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

import React from 'react';

export default {
	namespace: 'dragVerification',
	state: {
		y: 0,
		array: [],
		imgX: 0,
		imgY: 0,
		small: '',
		normal: '',
		token: '',
		errorCount: 0,
		success: false,
		myX: 0
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
		*getDragVerificationCode({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.DragVerificationApi().appDragVerificationGetDragVerificationCode,
					payload: {
						...payload
					}
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						...result,
						errorCount: 0,
						success: false
					}
				});
			}
		},
		*checkCode({ payload }, { call, put, select }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.DragVerificationApi().appDragVerificationCheckCode,
					payload: {
						...payload
					}
				})
			);
			if (success) {
				if (result.success) {
					yield put({
						type: 'setState',
						payload: {
							success: true,
							token: result.token,
							errorCount: 0,
							myX: payload.point
						}
					});
                    payload.callback(result.token);
				} else {
					let errorCount = yield select(({ dragVerification }) => dragVerification.errorCount);
					errorCount++;
					if (errorCount >= 4) {
						yield put({
							type: 'getDragVerificationCode'
						});
					} else {
						yield put({
							type: 'setState',
							payload: {
								errorCount: errorCount
							}
						});
					}
				}
			}
		}
	}
};
