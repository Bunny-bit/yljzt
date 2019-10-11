import moment from 'moment';
import * as api from './../api/api';
import { createApiAuthParam } from './../api/apiUtil.js';

export default {
    namespace: 'datitu',
    state: {
        data: [{
            xingming: '张三',
            timu: 38
        }, {
            xingming: '李四',
            timu: 318
        }, {
            xingming: '王二',
            timu: 138
        }],
        value: [{
            xueyuan: '互联网',
            zhengquelv: 38,
            zanbi:0.38
        }, {
            xueyuan: '酒店',
            zhengquelv: 22,
            zanbi:0.22
        }, {
            xueyuan: '经管',
            zhengquelv: 40,
            zanbi:0.40
        }],
        shuju: [{
            timu: '第一题',
            daduirengshu: 58
        }, {
            timu: '第二题',
            daduirengshu: 22
        }, {
            timu: '第三题',
            daduirengshu: 40
        }],
        cyrs: [{
            xueyuan: '互联网',
            canyurengshu: 58
        }, {
            xueyuan: '酒店',
            canyurengshu: 38
        }, {
            xueyuan: '经管',
            canyurengshu: 48
        }],
        forceFit: true,
        width: 500,
        height: 450,
        plotCfg: {
            margin: [20, 60, 80, 120]
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
