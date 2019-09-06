import React from 'react';
import styles from './../AuditLog.css';
import {Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Modal, Tooltip} from 'antd';
const create = Form.create;
const Search = Input.Search;

import {connect} from 'dva';

function AuditLogInfo({dispatch, expand, form, loading, visible, record}) {
  function handleCancel() {
    dispatch({
      type: "auditLog/setState",
      payload: {
        visible: !visible,
      }
    });
  }

  function getFormattedParameters(parameters) {
    try {
      var json = JSON.parse(parameters);
      return JSON.stringify(json, null, 4);
    } catch (e) {
      return parameters;
    }
  }

  return (
    <Modal
      visible={visible}
      width={900}
      title="系统日志详情"
      onCancel={handleCancel}
      footer={[
        <Button key="submit" loading={loading} onClick={handleCancel}>取消</Button>,
      ]}
    >
      <div>
        <Row type="flex" gutter={12}>
          <Col span={12} className={styles.row3}>
            <h2>用户信息</h2>
            <Row gutter={24} type="flex">
              <Col span={4}>用户名:</Col>
              <Col span={20}>{record.userName}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>客户端:</Col>
              <Col span={20}>{record.clientName}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>IP地址:</Col>
              <Col span={20}>{record.clientIpAddress}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>浏览器:</Col>
              <Col span={20}>{record.browserInfo}</Col>
            </Row>
            <h2>自定义数据</h2>
            <Row gutter={24} type="flex">
              {
                record.customData == null ?
                  <Col span={24}>没有自定义数据</Col>
                  :
                  <Col span={24}>
                    <pre>{record.customData}</pre>
                  </Col>
              }
            </Row>
          </Col>
          <Col span={12} className={styles.row3}>
            <h2>操作信息</h2>
            <Row gutter={24} type="flex">
              <Col span={4}>Service:</Col>
              <Col span={20}>{record.serviceName}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>Action:</Col>
              <Col span={20}>{record.methodName}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>操作时间:</Col>
              <Col span={20}>{record.executionTime}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>相应时长:</Col>
              <Col span={20}>{record.executionDuration + 'ms'}</Col>
            </Row>
            <Row gutter={24} type="flex">
              <Col span={4}>参数:</Col>
              <Col span={20}>
                <pre long="js">{getFormattedParameters(record.parameters)}</pre>
              </Col>
            </Row>
          </Col>
          <Col span={24} className={styles.row3}>
            <h2>错误状态</h2>
            <Row gutter={24} type="flex">
              {
                record.exception == null ?
                  <Col span={24}>成功</Col>
                  :
                  <Col span={24}>
                    <pre>{record.exception}</pre>
                  </Col>
              }
            </Row>
          </Col>
        </Row>
      </div>
    </Modal>
  );
}

AuditLogInfo = connect((state) => {
  return {
    ...state.auditLog,
  }
})(AuditLogInfo);

export default create()(AuditLogInfo);
