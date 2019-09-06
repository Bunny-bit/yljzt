import * as service from '../services/service';
import { message, Modal, notification } from 'antd';
import '../utils/jquery-vendor';
import 'signalr';
import { remoteUrl } from '../utils/url';
import { routerRedux } from 'dva/router';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

export default {
	namespace: 'home',
	state: {
		title: '',
		pathname: '',
		collapsed: false,
		loading: false,
		visible: false,
		changePassword: false,
		changeInformation: false,
		openChat: false,
		openSearchFriend: false,
		friendPagination: {
			filter: '',
			skipCount: 0,
			maxResultCount: 10,
			pageSize: 10,
			pageIndex: 0,
			total: 0,
			current: 1,
			showSizeChanger: true,
			showQuickJumper: true,
			showTotal: (total) => `共 ${total} 条`,
			size: 'large'
		},
		showtexts: '',
		refDom: '',
		sendvalue: '',
		mybbtodo: [],
		allmessage: [],
		havemessage: false,
		addselect: [],
		shownew: '0',
		mintalk: true,
		display: false,
		morenshow: 'niyani',
		touxiang: 'http://img.jsqq.net/uploads/allimg/150210/1-150210161I90-L.jpg',
		previewVisible: false,
		previewImage: '',
		fileList: [
			{
				uid: -1,
				name: 'xxx.png',
				status: 'done',
				url: 'http://img.jsqq.net/uploads/allimg/150210/1-150210161I90-L.jpg'
			}
		],
		permissions: [],
		menus: [],
		skins: [
			{ name: '默认', value: '#108ee9' },
			{ name: '百草霜', value: '#303030' },
			{ name: '柏坊灰蓝', value: '#4e1892' },
			{ name: '宝蓝', value: '#1f3696' },
			{ name: '北京毛蓝', value: '#276893' },
			{ name: '碧玉石', value: '#569597' },
			{ name: '苍黄', value: '#c65306' },
			{ name: '藏蓝', value: '#25386b' },
			{ name: '苍绿', value: '#4e5f45' },
			{ name: '草黄', value: '#dbce54' },
			{ name: '承德灰', value: '#757570' },
			{ name: '承德皂', value: '#5a5c5b' },
			{ name: '辰砂', value: '#af5e53' },
			{ name: '春蓝', value: '#7ba1a8' },
			{ name: '春绿', value: '#e3efd1' },
			{ name: '翠绿', value: '#006e5f' },
			{ name: '粗晶皂', value: '#43454a' },
			{ name: '大赤金', value: '#6d7358' },
			{ name: '黛蓝', value: '#304758' },
			{ name: '丹东石', value: '#d7c16b' },
			{ name: '淡灰绿', value: '#aec4b7' },
			{ name: '灯草灰', value: '#363532' },
			{ name: '靛蓝', value: '#1b54f2' },
			{ name: '蕃茄红', value: '#c4473d' },
			{ name: '妃红', value: '#c35655' },
			{ name: '甘草黄', value: '#e4cf8e' },
			{ name: '橄榄绿', value: '#6a6834' },
			{ name: '甘石粉', value: '#eadcd6' },
			{ name: '钴蓝', value: '#6493af' },
			{ name: '果灰', value: '#88aea3' },
			{ name: '海蓝', value: '#17507d' },
			{ name: '红皂', value: '#4f5355' },
			{ name: '黄灰', value: '#b0b7ac' },
			{ name: '花青', value: '#546b83' },
			{ name: '胡粉', value: '#ebe8db' },
			{ name: '灰蓝', value: '#5d828a' },
			{ name: '灰绿', value: '#5c8987' },
			{ name: '灰米', value: '#b6b196' },
			{ name: '姜黄', value: '#b49436' },
			{ name: '将校呢', value: '#6d614a' },
			{ name: '绛紫', value: '#704d4e' },
			{ name: '桔红', value: '#e7693f' },
			{ name: '桔黄', value: '#e8853b' },
			{ name: '金黄', value: '#c77a3a' },
			{ name: '军绿', value: '#cad4ba' },
			{ name: '孔雀蓝', value: '#0041a5' },
			{ name: '库金', value: '#85794f' },
			{ name: '枯绿', value: '#b7b278' },
			{ name: '蜡白', value: '#e7e5d0' },
			{ name: '老绿', value: '#3d6e53' },
			{ name: '榴花红', value: '#d54b44' },
			{ name: '芦灰', value: '#a9b08f' },
			{ name: '玫瑰红', value: '#973444' },
			{ name: '玫瑰灰', value: '#793d56' },
			{ name: '米红', value: '#e1bda2' },
			{ name: '米灰', value: '#c5bfad' },
			{ name: '米色', value: '#f5f5dc' },
			{ name: '奶绿', value: '#afc8ba' },
			{ name: '奶棕', value: '#c1a299' },
			{ name: '柠檬黄', value: '#e9db39' },
			{ name: '品红', value: '#a71368' },
			{ name: '浅海昌蓝', value: '#3c5e91' },
			{ name: '浅黄棕', value: '#dea87a' },
			{ name: '浅桔黄', value: '#da9558' },
			{ name: '牵牛紫', value: '#a22076' },
			{ name: '浅石英紫', value: '#ab96c5' },
			{ name: '浅藤紫', value: '#c4c3cb' },
			{ name: '浅驼色', value: '#c9ae8c' },
			{ name: '浅血牙', value: '#eacdd1' },
			{ name: '浅棕灰', value: '#e1dbcd' },
			{ name: '卡其黄', value: '#d5b884' },
			{ name: '卡其绿', value: '#647370' },
			{ name: '茄皮紫', value: '#674950' },
			{ name: '鹊灰', value: '#455667' },
			{ name: '绒蓝', value: '#31678d' },
			{ name: '三绿', value: '#90caaf' },
			{ name: '沙绿', value: '#005b5a' },
			{ name: '沙青', value: '#2b5e7d' },
			{ name: '深烟', value: '#5a4c4c' },
			{ name: '深烟红', value: '#643441' },
			{ name: '深竹月', value: '#2578b5' },
			{ name: '十样锦', value: '#fcb1aa' },
			{ name: '水貂灰', value: '#949c97' },
			{ name: '水黄', value: '#bed2b6' },
			{ name: '藤黄', value: '#f2de76' },
			{ name: '天青*', value: '#2ec3e7' },
			{ name: '铁灰', value: '#37444b' },
			{ name: '土黄', value: '#ce9335' },
			{ name: '相思灰', value: '#625c52' },
			{ name: '血红', value: '#a03e28' },
			{ name: '猩红', value: '#c43739' },
			{ name: '雄黄', value: '#d0853d' },
			{ name: '雄精', value: '#e47542' },
			{ name: '锈红', value: '#4d1919' },
			{ name: '锈绿', value: '#b8c8b7' },
			{ name: '选金', value: '#796f54' },
			{ name: '雪色', value: '#fffafa' },
			{ name: '雪紫', value: '#79485a' },
			{ name: '鸭蛋青', value: '#d1e3db' },
			{ name: '洋葱紫', value: '#9c6680' },
			{ name: '洋红', value: '#dc143c' },
			{ name: '艳红', value: '#cc3536' },
			{ name: '鹦鹉绿', value: '#008e59' },
			{ name: '银朱', value: '#dd3b44' },
			{ name: '油绿', value: '#45554a' },
			{ name: '油烟墨', value: '#3f3f3c' },
			{ name: '元青', value: '#3e3c3d' },
			{ name: '胭脂', value: '#c03f3c' },
			{ name: '银箔', value: '#585a57' },
			{ name: '月季红', value: '#bb1c33' },
			{ name: '玉石蓝', value: '#507883' },
			{ name: '枣红', value: '#89303f' },
			{ name: '章丹', value: '#eb652d' },
			{ name: '正灰', value: '#93a2a9' },
			{ name: '枝黄', value: '#dbc7a6' },
			{ name: '织锦灰', value: '#748a8d' },
			{ name: '纸棕', value: '#bca590' },
			{ name: '中棕灰', value: '#a9987c' },
			{ name: '紫粉', value: '#a54358' },
			{ name: '紫水晶', value: '#c3a6cb' },
			{ name: '紫藤灰', value: '#857e95' },
			{ name: '紫薇花', value: '#eea5d1' },
			{ name: '棕茶', value: '#b8844f' }
		],
		result: {},
		friends: {},
		searchfriends: {},
		notification: {
			unreadCount: 0,
			items: []
		},
		visibleNotification: false,
		visibleNotificationPopover: false,
		notificationSettings: {
			receiveNotifications: false,
			notifications: []
		},
		chatLoading: false,
		shouldChangePasswordOnNextLogin: false
	},
	reducers: {
		setState(state, { payload }) {
			return {
				...state,
				...payload
			};
		},
		setTitle(state, { payload }) {
			let title = '';
			let pathname = payload.pathname || state.pathname;
			let menus = payload.menus || state.menus;
			var currentMenus = menus.filter((n) => n.url == pathname);
			if (currentMenus && currentMenus.length > 0) {
				title = currentMenus[0].displayName;
			}
			return {
				...state,
				title: title,
				pathname: pathname,
				menus: menus
			};
		},
		setMybbtodoState(state, { payload }) {
			return {
				...state,
				mybbtodo: [...state.mybbtodo, ...payload]
			};
		},
		setbadge(state, { payload }) {
			if (state.friends) {
				let newfriends = state.friends.friends.map((ele) => {
					if (ele.friendUserId == payload.message.targetUserId && payload.message.readState == 1) {
						return {
							...ele,
							badge: payload.shownew
						};
					} else {
						return ele;
					}
				});
				return {
					...state,
					friends: {
						...state.friends,
						friends: [...newfriends]
					}
				};
			}
		},
		setRend(state, { payload }) {
			if (state.friends) {
				let newfriends = state.friends.friends.map((ele) => {
					if (ele.friendUserId == payload.message.userId) {
						return {
							...ele,
							badge: payload.shownew
						};
					} else {
						return ele;
					}
				});
				return {
					...state,
					friends: {
						...state.friends,
						friends: [...newfriends]
					}
				};
			}
		},

		setNotificationSettingsState(state, { payload }) {
			return {
				...state,
				notificationSettings: {
					...state.notificationSettings,
					...payload
				}
			};
		},
		pushRealtimeNotification(state, { payload }) {
			var notifications = [...state.notification.items];
			notifications.unshift(payload);
			if (notifications.length > 3) {
				notifications.pop();
			}

			return {
				...state,
				notification: {
					...state.notification,
					unreadCount: state.notification.unreadCount + 1,
					items: notifications
				}
			};
		}
	},
	effects: {
		*initialize({ payload }, { call, put }) {

			yield put({
				type: 'getColor'
			});
			yield put({
				type: 'getUserPermissions'
			});
			yield put({
				type: 'getUserMenus'
			});
			yield put({
				type: 'getssion'
			});
			yield put({
				type: 'getUserNotifications'
			});
			yield put({
				type: 'indexpage/GetClientSetting'
			});
			yield put({
				type: 'signal'
			});
		},
		*getColor({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ConfigurationApi().appConfigurationGetUiTheme,
				})
			);
			if (success) {
				yield put({
					type: 'changeUiColor',
					payload: result.name
				});
			}
		},
		*getUserPermissions({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.PermissionApi().appPermissionGetUserPermissions,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						permissions: result
					}
				});
			}
		},
		*getUserMenus({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.MenuApi().appMenuGetUserMenus,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setTitle',
					payload: {
						menus: result
					}
				});
			}
		},
		*getssion({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.SessionApi().appSessionGetCurrentLoginInformations,
					payload: payload
				})
			);
			if (success) {
				//console.log(result.user.id);
				yield put({
					type: 'setState',
					payload: {
						result: result
					}
				});
			}
		},

		*getGetSkins({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ConfigurationApi().appConfigurationGetSkinList,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						skins: result.items
					}
				});
			}
		},

		*getUserNotifications({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.NotificationApi().appNotificationGetUserNotifications,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						notification: {
							...result
						}
					}
				});
			}
		},
		*changession({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserUpdateCurrentUser,
					payload: payload
				})
			);
			if (success) {
				Modal.success({
					title: '修改成功'
				});
				yield put({
					type: 'setState',
					payload: {
						changeInformation: false
					}
				});
			}
		},
		*changepassword({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ProfileApi().appProfileChangePassword,
					payload: payload
				})
			);
			if (success) {
				Modal.success({
					title: '修改成功'
				});
				yield put({
					type: 'setState',
					payload: {
						changePassword: false,
						shouldChangePasswordOnNextLogin: false
					}
				});
			}
		},

		*setAllNotificationsAsRead({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.NotificationApi().appNotificationSetAllNotificationsAsRead,
					payload: payload
				})
			);
			if (success) {
				message.success('标记成功');

				yield put({
					type: 'getUserNotifications'
				});
			}
		},

		*getNotificationSettings({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.NotificationApi().appNotificationGetNotificationSettings,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						notificationSettings: {
							...result
						}
					}
				});
			}
		},

		*updateNotificationSettings({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.NotificationApi().appNotificationUpdateNotificationSettings,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						visibleNotification: false
					}
				});
				message.success('保存成功');
			}
		},

		*signal({ payload }, { call, put }) {
			yield put({
				type: 'setState',
				payload: {
					chatLoading: true
				}
			});
			if (!$.connection.abpCommonHub) {
				const result = yield call(service.signalrjs);
				eval(result);
			}

			$.connection.hub.disconnected(function () {
				setTimeout(function () {
					$.connection.hub.start();
				}, 5000);
			});

			$.connection.abpCommonHub.client.getNotification = function (notificationInfo) {
				console.log(notificationInfo);

				notification.info({
					message:
						notificationInfo.notification.data && notificationInfo.notification.data.properties.message
							? notificationInfo.notification.data.properties.message
							: '系统通知',
					description:
						notificationInfo.notification.data && notificationInfo.notification.data.properties.content
							? notificationInfo.notification.data.properties.content
							: notificationInfo.notification.data.properties.message
				});
				window.dispatch({
					type: 'home/pushRealtimeNotification',
					payload: notificationInfo
				});
			};
			$.connection.chatHub.client.getChatMessage = function (message) {
				console.log('app.chat.messageReceived', message);
				window.dispatch({
					type: 'home/setMybbtodoState',
					payload: [message]
				});

				window.dispatch({
					type: 'home/setbadge',
					payload: {
						message: message,
						shownew: 'new'
					}
				});
				window.dispatch({
					type: 'home/setState',
					payload: {
						havemessage: true,
						messageor: message
					}
				});
			};

			$.connection.chatHub.client.getAllFriends = function (friends) {
				console.log('abp.chat.friendListChanged', friends);
			};

			$.connection.chatHub.client.getFriendshipRequest = function (friendData, isOwnRequest) {
				console.log('app.chat.friendshipRequestReceived', friendData, isOwnRequest);
			};

			$.connection.chatHub.client.getUserConnectNotification = function (friend, isConnected) {
				console.log('app.chat.userConnectionStateChanged', {
					friend: friend,
					isConnected: isConnected
				});
			};

			$.connection.chatHub.client.getUserStateChange = function (friend, state) {
				console.log('app.chat.userStateChanged', {
					friend: friend,
					state: state
				});
			};

			$.connection.chatHub.client.getallUnreadMessagesOfUserRead = function (friend) {
				console.log('app.chat.allUnreadMessagesOfUserRead', {
					friend: friend
				});
			};

			//Connect to the server
			$.connection.hub.start().done(function () {
				window.dispatch({
					type: 'home/setState',
					payload: {
						chatLoading: false
					}
				});
				console.log('Connected to SignalR server!'); //TODO: Remove log
			});
		},
		*sendMessage({ payload }, { call, put }) {
			if ($.connection.hub.state !== $.signalR.connectionState.connected) {
				message.warning('没有连接到聊天服务器');
				return;
			}

			$.connection.chatHub.server.sendMessage(payload).done(function (result) {
				if (result) {
					console.log(result);
				}
			});
		},

		*logoutmy({ payload }, { call, put }) {
			const { success, result } = yield call(service.logoutmy, {
				method: 'post'
			});
			yield put(routerRedux.push('/'));
			localStorage.clear();
			message.success('退出成功');
		},
		*getfriends({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ChatApi().appChatGetUserChatFriendsWithSettings,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						friends: result
					}
				});
			}
		},
		*searchfriend({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.FriendshipApi().appFriendshipGetCreateFriendshipUserList,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						searchfriends: result,
						friendPagination: {
							filter: payload.filter,
							skipCount: payload.skipCount,
							maxResultCount: payload.maxResultCount,
							pageSize: payload.maxResultCount,
							pageIndex: payload.skipCount / payload.maxResultCount,
							total: result.totalCount
						}
					}
				});
			}
		},
		*addfriend({ payload }, { call, put, select }) {
			var friendPagination = yield select(({ home }) => home.friendPagination);
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.FriendshipApi().appFriendshipCreateFriendshipRequest,
					payload: payload
				})
			);
			if (success) {
				message.success('添加成功');
				yield put({
					type: 'searchfriend',
					payload: {
						skipCount: friendPagination.pageSize * ((friendPagination.current || 1) - 1),
						maxResultCount: friendPagination.pageSize,
						filter: friendPagination.filter
					}
				});
				yield put({
					type: 'getfriends'
				});
			}
		},
		*blockuser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.FriendshipApi().appFriendshipBlockUser,
					payload: payload
				})
			);
			if (success) {
				message.success('拉黑成功');
				yield put({
					type: 'getfriends'
				});
			}
		},
		*unblockuser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.FriendshipApi().appFriendshipUnblockUser,
					payload: payload
				})
			);
			if (success) {
				message.success('取消拉黑成功');
				yield put({
					type: 'getfriends'
				});
			}
		},
		*getchatmessage({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ChatApi().appChatGetUserChatMessages,
					payload: payload
				})
			);
			yield put({
				type: 'setState',
				payload: {
					allmessage: result,
					shownew: '0'
				}
			});
			yield put({
				type: 'rendif',
				payload: {
					tenantId: payload.tenantId,
					userId: payload.userId
				}
			});
			yield put({
				type: 'setRend',
				payload: {
					message: payload,
					shownew: '0'
				}
			});

			// console.log(result)
		},
		*rendif({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ChatApi().appChatMarkAllUnreadMessagesOfUserAsRead,
					payload: payload
				})
			);
		},
		*anyaddfriend({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.FriendshipApi().appFriendshipBatchCreateFriendshipRequest,
					payload: payload
				})
			);
			if (success) {
				message.success('添加成功');
				yield put({
					type: 'getfriends'
				});
			}
		},
		*changeUiColor({ payload }, { call, put }) {
			if (payload === '#108ee9') {
				if (window.less) {
					window.less
						.modifyVars({
							'@primary-color': payload
						});
				}
			} else {
				if (!window.less) {
					let head = document.getElementsByTagName('head').item(0);
					let link = document.createElement('link');
					link.type = 'text/css';
					link.id = 'less';
					link.rel = 'stylesheet/less';
					link.href = '/color/color.less';
					head.appendChild(link);

					const lessjs = yield call(service.getLessjs);
					eval(lessjs);
				}

				window.less
					.modifyVars({
						'@primary-color': payload
					});
			}
		},
		*changeStorageAndUiColor({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ConfigurationApi().appConfigurationChangeUiTheme,
					payload: { theme: payload }
				})
			);

			if (success) {
				yield put({
					type: 'changeUiColor',
					payload: payload
				});
				message.success('修改主题颜色成功');
			}
		}
	},
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				dispatch({
					type: 'setTitle',
					payload: {
						pathname: pathname
					}
				});
			});
		}
	}
};
