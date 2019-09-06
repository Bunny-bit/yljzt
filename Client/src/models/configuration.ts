import { Modal } from 'antd';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil';

export default {
	namespace: 'configuration',
	state: {
		allSetting: [],
		passwordComplexitySetting: {},
		mode: 'custom'
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
		*getAllSettings({ payload }, { select, call, put }) {
			let body = yield select(({ configuration }) => {
				return { mod: configuration.mode };
			});
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ConfigurationApi().appConfigurationGetAllSettings,
					payload: payload
				})
			);
			if (success) {
				let passwordComplexitySetting = {};
				if (
					result.filter((s) => s.name == 'SecuritySettingDto').length > 0 &&
					result
						.filter((s) => s.name == 'SecuritySettingDto')[0]
						.setting.filter((s) => s.name == 'PasswordComplexity').length > 0
				) {
					let json = result
						.filter((s) => s.name == 'SecuritySettingDto')[0]
						.setting.filter((s) => s.name == 'PasswordComplexity')[0].value;
					passwordComplexitySetting = JSON.parse(json);
				}
				yield put({
					type: 'setState',
					payload: {
						allSetting: result,
						passwordComplexitySetting: passwordComplexitySetting
					}
				});
			}
		},
		*updateAllSettings({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ConfigurationApi().appConfigurationUpdateAllSettings,
					payload: payload
				})
			);
			if (success) {
				Modal.success({
					title: '保存成功'
				});
			}
		}
	},
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				if (pathname.toLowerCase() == '/configuration'.toLowerCase()) {
					dispatch({
						type: 'getAllSettings'
					});
				}
			});
		}
	}
};
