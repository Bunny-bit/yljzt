import moment from 'moment';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

export default {
    namespace: 'bingtu',
    state: {
        // data: [{
        //     timu: '第一题',
        //     zhengquelv: 38
        //   }, {
        //     timu: '第二题',
        //     zhengquelv: 22
        //   }, {
        //     timu: '第三题',
        //     zhengquelv: 40
        //   },{
        //     timu: '第四题',
        //     zhengquelv: 40
        //   }
        //   ,{
        //     timu: '第五题',
        //     zhengquelv: 40
        //   }, {
        //     timu: '第二题',
        //     zhengquelv: 22
        //   }, {
        //     timu: '第三题',
        //     zhengquelv: 40
        //   },{
        //     timu: '第四题',
        //     zhengquelv: 40
        //   }
        // ],
        valuce: [{
            xueyuan: '互联网',
            zhengquelv: 38
        }, {
            xueyuan: '互',
            zhengquelv: 16
        }, {
            xueyuan: '互联',
            zhengquelv: 26
        }, {
            xueyuan: '互联网学',
            zhengquelv: 5
        }, {
            xueyuan: '互联网学院',
            zhengquelv: 15
        }
        ],
        forceFit: true,
        width: 500,
        height: 450,
        plotCfg: {
            margin: [20, 60, 80, 120, 140]
        },
    },
    reducers: {
        setState(state, { payload }) {
            return {
                ...state,
                ...payload
            }
        }
    },
    effects: {
        *getUserLogins({ payload }, { call, put }) {
            const { success, result } = yield call(...createApiAuthParam({
                method: new api.RoleApi().appRoleGetRoles,//接入数据
                payload: payload
            }));
            if (success) {
                yield put({
                    type: 'setState',
                    payload: {
                        data: result.items,
                    }
                });
            }
        },
    },
    subscriptions: {},
};