import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from './Activation.css';
import Logo from "../../assets/logologin.gif";
function Activation({dispatch, form, children}) {
  return (
    <div className={styles.navbox}>
      <div className={styles.logologin}><img src={Logo}/></div>
      <div className={styles.indexbox}>
        {/*<header className={styles.headerbox}>*/}
        {/*<span className={styles.headcol}>发送激活邮件</span>*/}
        {/*<Link to='/' className={styles.headback}>返回登录</Link>*/}
        {/*</header>*/}
        {children}
      </div>
    </div>
  )
}
Activation = connect((state) => {
  return {
    ...state.Activation,
  }
})(Activation);

export default Activation;
