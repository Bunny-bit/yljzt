import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button} from 'antd';
import styles from './Findpass.css';
import Logo from "../../assets/logologin.gif";
function Findpass({dispatch, form, children}) {
  return (
    <div className={styles.navbox}>
      <div className={styles.logologin}><img src={Logo}/></div>
      <div className={styles.indexbox}>
        <header className={styles.headerbox}><span className={styles.headcol}>找回密码</span><Link to='/'
                                                                                               className={styles.headback}>返回登录</Link>
        </header>
        <div>{children}</div>
      </div>
    </div>
  )
}
Findpass = connect((state) => {
  return {
    ...state.backkonw,
  }
})(Findpass);

export default Findpass;
