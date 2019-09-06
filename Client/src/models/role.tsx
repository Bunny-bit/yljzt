import React from 'react';
import {message} from "antd";
import {notification} from "antd";

import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';

export default {
  namespace: 'role',
  state: {
    addRoleModalState: {visible: false, activeTabKey: "1"},
    editRoleModalState: {visible: false, activeTabKey: "1"},
    roles: [],
    editingRole: {},
    permissionTree: [],
    selectRoles: []
  },
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      }
    },
  },
  effects: {
    *getRoles({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.RoleApi().appRoleGetRoles,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            roles: result.items,
          }
        });
      }
    },
    *getRoleForEdit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.RoleApi().appRoleGetRoleForEdit,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            editingRole: result,
            selectRoles: result.grantedPermissionNames,
            editRoleModalState: {visible: true, activeTabKey: "1"}
          }
        });
      }
    },
    *createOrUpdateRole({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.RoleApi().appRoleCreateOrUpdateRole,
        payload: payload
      }));
      console.log(success)
      console.log(result)
      if (success) {
        if (!payload.role.id) {
          notification.success({
            message: "添加成功!",
            description: <span>您已成功添加角色<strong>{payload.role.displayName}</strong></span>
          });
        } else {
          notification.success({
            message: "修改成功!",
            description: <span>您已成功修改角色<strong>{payload.role.displayName}</strong></span>
          });
        }
        yield put({
          type: 'setState',
          payload: {
            addRoleModalState: {visible: false, activeTabKey: "1"},
            editRoleModalState: {visible: false, activeTabKey: "1"}
          }
        })
        yield put({
          type: 'getRoles',
          payload: {}
        });
      }
    },
    *deleteRole({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.RoleApi().appRoleDeleteRole,
        payload: payload
      }));
      if (success) {
        notification.success({
          message: "删除成功!",
          description: <span>您已成功删除角色<strong>{payload.displayName}</strong></span>
        });
        yield put({
          type: 'getRoles',
          payload: {}
        });
      }
    },
    *getAllPermissionTree({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.PermissionApi().appPermissionGetAllPermissionTree,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            permissionTree: result,
          }
        });
      }
    },
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/role'.toLowerCase()) {
          dispatch({
            type: 'getAllPermissionTree',
            payload: {}
          })
          dispatch({
            type: 'getRoles',
            payload: {
              current: 1,
              pageSize: 10
            }
          });
        }
      });
    },
  },
};
