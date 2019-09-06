import React, {Component, PropTypes} from 'react';
import {connect} from 'dva';
import {Link} from 'dva/router';
import {Form, Icon, Input, Button, Checkbox, message} from 'antd';
import styles from './Emailregister.css';
import Logo from "../../assets/logologin.gif";
const FormItem = Form.Item;
const create = Form.create;
function Emailregister({dispatch, form, children}) {
  const {getFieldDecorator} = form;

  return (
    <div className={styles.navbox}>
      <div className={styles.logologin}><img src={Logo}/></div>
      <div className={styles.indexbox}>
        <header className={styles.headerbox}><span className={styles.headcol}>新用户注册</span><Link to='/'
                                                                                                className={styles.headback}>返回登录</Link>
        </header>
        <div>{children}</div>
      </div>
    </div>
  )
}
Emailregister = connect((state) => {
  return {
    ...state.emailregister,
  }
})(Emailregister);

export default create()(Emailregister);
