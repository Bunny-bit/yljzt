import React from 'react';
import {notification} from "antd";

import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';

export default {
  namespace: 'organization',
  state: {
    organizations: [],
    userList: {},
    isAddOrganization: false,
    editOrganizationModalVisible: false,
    userPagination: {
      pageSize: 5,
      pageIndex: 0,
      total: 0,
      current: 1,
      showSizeChanger: true,
      showQuickJumper: true,
      showTotal: total => `共 ${total} 条`,
      size: 'large'
    },
    userLoading: false,
    organization: {},
    editOrganizationId: 0,
    parentId: 0,
    validUserList: {},
    addOrganizationUserModalVisible: false,
    validUserPagination: {
      pageSize: 5,
      pageIndex: 0,
      total: 0,
      current: 1,
      showSizeChanger: true,
      showQuickJumper: true,
      showTotal: total => `共 ${total} 条`,
      size: 'large'
    },
    validUserLoading: false,
    validUserNameFilter: '',
    selectedUsers: []
  },
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      };
    }
  },
  effects: {
    *getOrganizationUnits({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitGetOrganizationUnits,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            organizations: result.items
          }
        });
      }
    },
    *createOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitCreateOrganizationUnit,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'getOrganizationUnits',
        });
        yield put({
          type: 'setState',
          payload: {
            editOrganizationModalVisible: false
          }
        });
        notification.success({
          message: "添加成功!",
          description: <span>您已成功创建组织机构<strong>{payload.displayName}</strong></span>
        });
      }
    },
    *updateOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitUpdateOrganizationUnit,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'getOrganizationUnits',
        });
        yield put({
          type: 'setState',
          payload: {
            editOrganizationModalVisible: false
          }
        });
        notification.success({
          message: "修改成功!",
          description: <span>您已成功修改组织机构</span>
        });
      }
    },
    *moveOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitMoveOrganizationUnit,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'getOrganizationUnits',
        });
        notification.success({
          message: "修改成功!",
          description: <span>您已成功修改组织机构</span>
        });
      }
    },
    *deleteOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitDeleteOrganizationUnit,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'getOrganizationUnits',
        });
        // 删除机构后，又侧的用户列表也清空
        yield put({
          type: 'setState',
          payload: {
            userList: { items: [] },
            userLoading: false,
            userPagination: {
              pageSize: 0,
              pageIndex: 1,
              total: 0
            }
          }
        });
        notification.success({
          message: "删除成功!",
          description: <span>您已成功删除组织机构</span>
        });
      }
    },
    *getOrganizationUnitUsers({payload}, {call, put}) {
      yield put({
        type: 'setState',
        payload: {
          userLoading: true
        }
      });
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitGetOrganizationUnitUsers,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            userList: result,
            userLoading: false,
            userPagination: {
              pageSize: payload.maxResultCount,
              pageIndex: payload.skipCount / payload.maxResultCount,
              total: result.totalCount
            }
          }
        });
      }
    },
    *getOrganizationUnitJoinableUserList({payload}, {call, put}) {
      yield put({
        type: 'setState',
        payload: {
          validUserLoading: true
        }
      });
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitGetOrganizationUnitJoinableUserList,
        payload: payload
      }));
      if (success) {
        yield put({
          type: 'setState',
          payload: {
            validUserList: result,
            validUserLoading: false,
            validUserPagination: {
              pageSize: payload.maxResultCount,
              pageIndex: payload.skipCount / payload.maxResultCount,
              total: result.totalCount
            }
          }
        });
      }
    },
    *addUserToOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitAddUserToOrganizationUnit,
        payload: payload
      }));
      if (success) {
        notification.success({
          message: "操作成功!",
          description: <span>用户加入组织机构成功!</span>
        });
        yield put({
          type: 'setState',
          payload: {
            addOrganizationUserModalVisible: false,
            selectedUsers: []
          }
        });
        yield put({
          type: 'getOrganizationUnitUsers',
          payload: {
            id: payload.organizationUnitId,
            maxResultCount: 5,
            skipCount: 0,
            isRecursiveSearch: false
          }
        });
      }
    },
    *removeUserFromOrganizationUnit({payload}, {call, put}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.OrganizationUnitApi().appOrganizationUnitRemoveUserFromOrganizationUnit,
        payload: payload
      }));
      if (success) {
        notification.success({
          message: "操作成功!",
          description: <span>从组织机构移除用户成功!</span>
        });
        yield put({
          type: 'getOrganizationUnitUsers',
          payload: {
            id: payload.organizationUnitId,
            maxResultCount: 5,
            skipCount: 0,
            isRecursiveSearch: false
          }
        });
      }
    },
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/organization'.toLowerCase()) {
          dispatch({
            type: 'getOrganizationUnits'
          });
        }
      });
    },
  },
};
