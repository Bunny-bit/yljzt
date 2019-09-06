import { routerRedux } from 'dva/router';
import { remoteUrl } from '../utils/url';
import { notification } from 'antd';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';
export default {
	namespace: 'register',
	state: {
		captcha: `${remoteUrl}/Captcha/GetCaptchaImage?t=${Math.random()}`,
		abled: false,
		hedhtml: '获取验证码'
	},
	subscriptions: {
		setup({ dispatch, history }) {}
	},

	effects: {
		*sendPhoneNumberCode({ payload }, { call, put }) {
			const data = yield call(
              ...createApiAuthParam({
                method: new api.RegisterApi().appRegisterSendPhoneNumberCode,
					payload: payload
				})
			);
			if (data.success) {
			} else {
			}
		},
		*register({ payload }, { call, put }) {
			const data = yield call(
              ...createApiAuthParam({
                method: new api.RegisterApi().appRegisterRegisterByPhoneNumber,
					payload: payload
				})
			);
			if (data.success) {
				notification.success({
					message: '注册成功',
					description: '恭喜你注册成功'
				});
				yield put(routerRedux.push('/callsucess'));
			} else {
				notification.error({
					message: '注册失败',
					description: '很遗憾注册失败'
				});
			}
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
};
