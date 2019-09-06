import React from 'react';
import {notification} from "antd";
import {routerRedux} from 'dva/router';
import * as service from '../services/service';
import { homePageUrl } from '../utils/url';

export default {
  namespace: 'thirdpartybinding',
  state: {
    bindinginfo: {},
    bindingModalVisible: false,
    bindingThirdPartyModalVisible:false,
    thirdPartyList:[],
    bindingResult:{isBinding:true,success:false,message:"",platform:""}
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
    *bindingThirdParty({payload}, {call, put}) {
      const data = yield call(service.bindingThirdParty, {
        method: 'post',
        body: payload
      });
      if (data.success) {
        notification.success({
          message: "绑定成功!",
          description: (<span>绑定成功，请前往登录页登录系统！</span>)
        });
        yield put(routerRedux.push(homePageUrl));
      } else {
        payload.callback();
      }
    },
    *getBindingThirdPartyList({payload}, {call, put}) {
      const data = yield call(service.getBindingThirdPartyList, {
        method: 'post',
        body: payload
      });
      if (data.success) {
        if(data.result&&data.result.length){
          yield put({
            type: 'setState',
            payload: {
              thirdPartyList: data.result,
              bindingThirdPartyModalVisible:true
            }
          });
        }else{
          notification.info({
            message: "当前系统没有可用的第三方平台!",
            description: (<span>当前系统没有可用的第三方平台,请联系管理员进行第三方平台配置！</span>)
          });
        }
      } else {
        payload.callback();
      }
    },
    *loginUserBindingThirdParty({payload}, {call, put}) {
      const data = yield call(service.loginUserBindingThirdParty, {
        method: 'post',
        body: payload
      });
      if (data.success) {
        yield put({
          type: 'setState',
          payload: {
            bindingResult: {...data.result,isBinding:false}
          }
        });
      } else {
        payload.callback();
      }
    },
    *loginUserUnbindingThirdParty({payload}, {call, put}) {
      const data = yield call(service.loginUserUnbindingThirdParty, {
        method: 'post',
        body: {thirdParty:payload.thirdParty}
      });
      if (data.success) {
        notification.success({
          message: "解绑成功!",
          description: (<span>您已成功解绑<strong>{payload.thirdPartyName}</strong>账号</span>)
        });
        yield put({
          type: 'getBindingThirdPartyList',
          payload: {}
        });
      } else {
        payload.callback();
      }
    },
  },
  subscriptions: {
    setup({dispatch, history}) {
      return history.listen(({pathname, state}) => {
        if (pathname.toLowerCase() == '/thirdpartybinding'.toLowerCase()) {
          dispatch({
            type: 'setState',
            payload: {bindingModalVisible: false}
          });
          dispatch({
            type: 'indexpage/setState',
            payload: {thirdPartyToken: localStorage.thirdPartyToken}
          });
          localStorage.thirdPartyToken = "";
        }
      });
    }
  }
};
