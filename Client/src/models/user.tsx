import React from 'react';
import { notification } from 'antd';

import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

import React from 'react';

export default {
	namespace: 'user',
	state: {
		expand: false,
		addUserModalState: { visible: false, activeTabKey: '1' },
		changeUserModalState: { visible: false, activeTabKey: '1' },
		changePasswordModalState: { visible: false, user: {} },
		changeUserPermissionModalState: { visible: false },
		items: [],
		roles: [], //角色列表
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
		filter: { name: '', userName: '', phoneNumber: '', filter: '' },
		organizations: [],
		organizationFilter: '',
		selectedOrganizations: [],
		visibleColumnTitles: [
			'用户名',
			'用户编号',
			'姓名',
			'角色',
			'邮箱地址',
			'手机号',
			'邮箱地址验证',
			'手机号码验证',
			'启用',
			'锁定',
			'上次登录时间',
			'创建时间',
			'操作'
		],
		visibleColumnWidth:1500,
		customColumnSelectorVisible: false,
		selectedUserIds: [],
		selectedUsers: [],
		currentEditUser: {
			roleCount: 0
		},
		currentEditUserPermission: {},
		selectPermission: [],
		currentUser: {}
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
		},
		setCurrentEditUserState(state, { payload }) {
			return {
				...state,
				currentEditUser: {
					...state.currentEditUser,
					...payload
				}
			};
		}
	},
	effects: {
		*getUsers({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserGetUsers,
					payload: {
						...payload,
						maxResultCount: payload.pageSize ? payload.pageSize : 10, //一页最多几条
						skipCount: payload.pageSize * (payload.current - 1) //跳过多少条
					}
				})
			);
			if (success) {
				yield put({
					type: 'setPages',
					payload: {
						items: result.items,
						pagination: {
							total: result.totalCount,
							current: payload.current,
							pageSize: payload.pageSize
						}
					}
				});
			}
		},
		*getUsersToExcel({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserGetUsersToExcel,
					payload: {
						...payload
					}
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
		},
		*createOrUpdateUser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserCreateOrUpdateUser,
					payload: payload
				})
			);
			if (success) {
				if (!payload.user.id) {
					notification.success({
						message: '添加成功!',
						description: (
							<span>
								您已成功添加用户<strong>{payload.user.name}</strong>
							</span>
						)
					});
					yield put({
						type: 'setState',
						payload: {
							addUserModalState: { visible: false, activeTabKey: '1' }
						}
					});
				} else {
					notification.success({
						message: '修改成功!',
						description: (
							<span>
								您已成功修改用户<strong>{payload.user.name}</strong>的信息
							</span>
						)
					});
					yield put({
						type: 'setState',
						payload: {
							changeUserModalState: { visible: false, activeTabKey: '1' }
						}
					});
				}
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*getRoles({}, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserGetRoles
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						roles: [ ...[], ...result.items ]
					}
				});
			}
		},
		*getOrganizationUnits({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.OrganizationUnitApi().appOrganizationUnitGetOrganizationUnits,
					payload: { value: payload.value }
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						organizations: result.items
					}
				});
			}
		},
		*batchDeleteUser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserBatchDeleteUser,
					payload: { value: payload.value }
				})
			);
			if (success) {
				notification.success({
					message: '删除成功!',
					description: (
						<span>
							您已成功删除<strong>{payload.users[0].name}</strong>等<strong>{payload.users.length}</strong>位用户
						</span>
					)
				});
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*batchOperate({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: payload.serverUrl,
					payload: {
						value: payload.value,
						Ids: payload.value,
						isActive: payload.isActive
					}
				})
			);
			if (success) {
				notification.success({
					message: `${payload.operate}成功!`,
					description: (
						<span>
							您已成功{payload.operate}
							<strong>{payload.users[0].name}</strong>等<strong>{payload.users.length}</strong>位用户
						</span>
					)
				});
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*batchUnlockUser({ payload }, { call, put }) {
			yield put({
				type: 'batchOperate',
				payload: {
					...payload,
					operate: '解锁',
					serverUrl: new api.UserApi().appUserBatchUnlockUser
				}
			});
		},
		*batchActiveUser({ payload }, { call, put }) {
			yield put({
				type: 'batchOperate',
				payload: {
					...payload,
					operate: '启用',
					isActive: true,
					serverUrl: new api.UserApi().appUserBatchActiveUser
				}
			});
		},
		*batchNotActiveUser({ payload }, { call, put }) {
			yield put({
				type: 'batchOperate',
				payload: {
					...payload,
					operate: '禁用',
					isActive: false,
					serverUrl: new api.UserApi().appUserBatchActiveUser
				}
			});
		},
		*deleteUser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserDeleteUser,
					payload: { id: payload.id }
				})
			);
			if (success) {
				notification.success({
					message: '删除成功!',
					description: (
						<span>
							用户<strong>{payload.name}</strong>已成功删除
						</span>
					)
				});
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*getUserForEdit({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserGetUserForEdit,
					payload: payload
				})
			);
			if (success) {
				var organizationIds = [];
				result.organizationIds.map((m) => organizationIds.push(m + ''));
				console.log(result.roles.filter((n) => n.isAssigned).length);
				result.roleCount = result.roles.filter((n) => n.isAssigned).length;
				yield put({
					type: 'setState',
					payload: {
						changeUserModalState: { visible: true, activeTabKey: '1' },
						currentEditUser: result,
						selectedOrganizations: organizationIds,
						organizationFilter: ''
					}
				});
			}
		},
		*getUserPermissionsForEdit({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserGetUserPermissionsForEdit,
					payload: payload
				})
			);
			if (success) {
				yield put({
					type: 'setState',
					payload: {
						changeUserPermissionModalState: { visible: true },
						currentEditUserPermission: result,
						selectPermission: result.grantedPermissionNames
					}
				});
			}
		},
		*updateUserPermissions({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserUpdateUserPermissions,
					payload: payload
				})
			);
			if (success) {
				notification.success({
					message: '修改权限成功!',
					description: ''
				});
				yield put({
					type: 'setState',
					payload: {
						changeUserPermissionModalState: { visible: false }
					}
				});
			}
		},
		*toggleActiveStatus({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserToggleActiveStatus,
					payload: { id: payload.id }
				})
			);
			if (success) {
				notification.success({
					message: '操作成功!',
					description: (
						<span>
							您已成功切换<strong>{payload.name}</strong>的{payload.activeName}状态
						</span>
					)
				});
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*unlockUser({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserUnlockUser,
					payload: { id: payload.id }
				})
			);
			if (success) {
				notification.success({
					message: '解锁成功!',
					description: (
						<span>
							您已成功解锁<strong>{payload.name}</strong>的登录锁定
						</span>
					)
				});
				yield put({
					type: 'getUsers',
					payload: { current: 1, pageSize: 10 }
				});
			}
		},
		*resetUserPassword({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ProfileApi().appProfileResetUserPassword,
					payload: { id: payload.id }
				})
			);
			if (success) {
				notification.success({
					message: '已经用户的密码重置为默认密码!',
					description: (
						<span>
							默认密码为<strong>{result}</strong>
						</span>
					)
				});
			}
		},
		*changeUserPassword({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.ProfileApi().appProfileChangeUserPassword,
					payload: {
						userId: payload.id,
						newPassword: payload.newPassword
					}
				})
			);
			if (success) {
				notification.success({
					message: '修改密码成功!',
					description: (
						<span>
							您已成功修改<strong>{payload.name}</strong>的登录密码
						</span>
					)
				});

				yield put({
					type: 'user/setState',
					payload: {
						changePasswordModalState: { visible: false }
					}
				});
			}
		},
		*resetUserSpecificPermissions({ payload }, { call, put }) {
			const { success, result } = yield call(
				...createApiAuthParam({
					method: new api.UserApi().appUserResetUserSpecificPermissions,
					payload: payload
				})
			);
			if (success) {
				notification.success({
					message: '权限恢复成功!',
					description: ''
				});
				yield put({
					type: 'getUserPermissionsForEdit',
					payload: {
						id: payload.id
					}
				});
			}
		}
	},
	subscriptions: {
		setup({ dispatch, history }) {
			return history.listen(({ pathname, state }) => {
				if (pathname.toLowerCase() == '/user'.toLowerCase()) {
					dispatch({
						type: 'getUsers',
						payload: {
							current: 1,
							pageSize: 10
						}
					});
					dispatch({
						type: 'getOrganizationUnits',
						payload: {}
					});
					dispatch({
						type: 'getRoles',
						payload: {}
					});
				}
			});
		}
	}
};
