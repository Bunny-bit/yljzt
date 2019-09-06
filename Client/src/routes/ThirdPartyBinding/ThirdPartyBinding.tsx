import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import styles from './ThirdPartyBinding.css';
import {Icon, Input, Button, Row, Col, Modal} from 'antd';
import Logo from "../../assets/logologin.gif";

import BindingModal from './modal/BindingModal';

function ThirdPartyBinding({dispatch, bindingModalVisible}) {

  function showBindingModel() {
    dispatch({
      type: "thirdpartybinding/setState",
      payload: {
        bindingModalVisible: true,
      }
    });
  }

  return (
    <div className={styles.navbox}>
      <div className={styles.logologin}><img src={Logo}/></div>
      <div className={styles.indexbox}>
        <header className={styles.headerbox}><span className={styles.headcol}>账号绑定</span></header>
        <div className={styles.formbox}>
          <Row className={styles.row}>
            <Col span={16}>使用邮箱注册账号：</Col>
            <Col span={8}>
              <Link to='/showres' className={styles.headback}>前往注册页面<Icon type="right"/></Link>
            </Col>
          </Row>
          <Row className={styles.row}>
            <Col span={16}>使用手机号码注册账号：</Col>
            <Col span={8}>
              <Link to='/register' className={styles.headback}>前往注册页面<Icon type="right"/></Link>
            </Col>
          </Row>
          <Row className={styles.row}>
            <Col span={16}>已有账号绑定：</Col>
            <Col span={8}><Button type="primary" onClick={showBindingModel}>立即绑定</Button></Col>
          </Row>
        </div>
      </div>

      {bindingModalVisible ? <BindingModal /> : null}
    </div>
  )
}
ThirdPartyBinding = connect((state) => {
  return {...state.thirdpartybinding}
})(ThirdPartyBinding);

export default ThirdPartyBinding;
