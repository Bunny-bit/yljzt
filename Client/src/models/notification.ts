import moment from 'moment';
import {message} from 'antd';
import * as api from './../api/api';
import {createApiAuthParam} from './../api/apiUtil.js';

export default {
  namespace: 'notification',
  state: {
    expand: false,
    visible: false,
    items: [],
    record: {
      notification: {data: {properties: {message: '', content: ''}}},
      state: 0
    },
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
    search: {},
    filters: {},
    sorter: {}
  },
  reducers: {
    setState(state, {payload}) {
      return {
        ...state,
        ...payload
      };
    },
    setPages(state, {payload}) {
      return {
        ...state,
        items: payload.items,
        pagination: {
          ...state.pagination,
          ...payload.pagination
        }
      };
    }
  },
  effects: {
    *getUserNotifications({payload}, {call, put}) {
      var body = {
        state: payload.state,
        sorting: payload.field ? payload.field + ' ' + payload.order.replace('end', '') : '',
        maxResultCount: payload.pageSize,
        skipCount: payload.pageSize * (payload.current - 1)
      };
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.NotificationApi().appNotificationGetUserNotifications,
        payload: body
      }));
      if (success) {
        yield put({
          type: 'setPages',
          payload: {
            items: result.items,
            pagination: {
              ...payload,
              total: result.totalCount,
              current: payload.current,
              pageSize: payload.pageSize
            }
          }
        });
      }
    },
    *setNotificationAsRead({payload}, {call, put, select}) {
      const {success, result} = yield call(...createApiAuthParam({
        method: new api.NotificationApi().appNotificationSetNotificationAsRead,
        payload: payload
      }));
      if (success) {
        message.success('已标记为已读');
        console.log(yield select(({notification}) => notification.pagination));
        yield put({
          type: 'getUserNotifications',
          payload: {
            ...(yield select(({notification}) => notification.pagination))
          }
        });
      }
    }
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/notification'.toLowerCase()) {
          dispatch({
            type: 'getUserNotifications',
            payload: {
              current: 1,
              pageSize: 10
            }
          });
          dispatch({
            type: 'home/setState',
            payload: {
              visibleNotificationPopover: false
            }
          });
        }
      });
    }
  }
};
