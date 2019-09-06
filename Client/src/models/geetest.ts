import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

import React from 'react';

export default {
	namespace: 'geetest',
	state: {},
	reducers: {
		setState(state, { payload }) {
			return {
				...state,
				...payload
			};
		}
	},
	effects: {
		*getCaptcha({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.GeetestApi().appGeetestGetCaptcha
				})
			);
			if (success) {
				payload.callback(JSON.parse(result));
			}
		},
		*check({ payload }, { call, put, select }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.GeetestApi().appGeetestCheck,
					payload: {
						...payload
					}
				})
			);
			if (success) {
				if (result.success) {
					payload.callback(result.token);
				}
			}
		},
		*reset({ payload }, { call, put, select }) {
			window.captchaObj.reset();
			window.captchaObj.props.onChange('');
		}
	}
};
