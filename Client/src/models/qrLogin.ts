import * as service from '../services/service';
import { routerRedux } from 'dva/router';
import $ from '../utils/jquery-vendor';
import signalr from 'signalr';
import { homePageUrl } from '../utils/url';

export default {
	namespace: 'qrLogin',
	state: {
		url: null,
		scanQRCode: false
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
		*init({ payload }, { call, put }) {
			if (!$.connection.qRLoginHub) {
				const result = yield call(service.signalrjs);
				eval(result);
			}

			$.connection.hub.disconnected(function() {
				setTimeout(function() {
					$.connection.hub.start();
				}, 5000);
			});

			$.connection.qRLoginHub.client.scanQRCode = function() {
				window.dispatch({
					type: 'qrLogin/setState',
					payload: {
						scanQRCode: true
					}
				});
			};
			$.connection.qRLoginHub.client.confirmLogin = function(token) {
				window.dispatch({
					type: 'qrLogin/qrLogin',
					payload: { token: token }
				});
			};

			//Connect to the server
			$.connection.hub.start().done(function() {
				window.dispatch({
					type: 'qrLogin/showQRCode'
				});
			});
		},
		*showQRCode({ payload }, { call, put }) {
			$.connection.qRLoginHub.server.getToken().done(function(result) {
				if (result) {
					console.log(result);
					window.dispatch({
						type: 'qrLogin/setState',
						payload: {
							url: `${location.protocol}//${location.host}/qrLogin.html?connectionId=${result.connectionId}&token=${result.token}`
						}
					});
				}
			});
			setTimeout(() => {
				window.dispatch({
					type: 'qrLogin/showQRCode'
				});
			}, 180000);
		},
		*qrLogin({ payload }, { call, put }) {
			const data = yield call(service.qrlogin, {
				method: 'post',
				body: payload
			});
			if (data.success) {
				yield put({
					type: 'setState',
					payload: {
						scanQRCode: false
					}
				});
				$.connection.hub.stop();
				yield put(routerRedux.push(homePageUrl));
			}
		}
	},
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				if (pathname.toLowerCase() == '/qrLogin'.toLowerCase()) {
					dispatch({
						type: 'init'
					});
				}
			});
		}
	}
};
