import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
import { Result, Icon, WhiteSpace } from 'antd-mobile';
import 'antd-mobile/dist/antd-mobile.css';
import styles from './tankuang.css';
const FormItem = Form.Item;
const create = Form.create;


function Tankuang({ dispatch, form }) {


    const myImg = src => <img src={src} className="spe am-icon am-icon-md" alt="" />;


    return (
        <div className={styles.boxss}>
            <Result
                className={styles.titless}
                img={myImg('http://pic.51yuansu.com/pic3/cover/01/59/31/594d771cf07ba_610.jpg ')}
                title="答题已完成"
            />
        </div>
    )

}

Tankuang = connect((state) => {
    return {

    };
})(Tankuang);

export default create()(Tankuang);