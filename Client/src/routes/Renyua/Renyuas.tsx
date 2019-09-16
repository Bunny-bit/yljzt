import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
const FormItem = Form.Item;
const create = Form.create;
import styles from './Renyuas.css';

function Renyuas({ dispatch, form }) {

    const { getFieldDecorator } = form;

    function handleSubmit(e) {

        form.validateFields((err, values) => {
            let data = {
                "name": "string",
                "xueyua": "string",
                "xuehao": 0,
                "banji": "string"
            };
            if (!err) {
                dispatch({
                    type: 'renyua/createrenyua',
                    payload: data
                });
            }
        });

    }

    const formCol = {
        labelCol: { span: 8 },
        wrapperCol: { span: 12 }
    };

    return (
        <div className={styles.container} >
            <img className={styles.images} src="http://www.ynctv.cn/ImageFile/GetPictureHvtThumbnailByPath?fileName=2/2019/6/18/logo.jpg" alt="" />
            <div className={styles.content} >
                <div className={styles.box}>
                    <header className={styles.contents}>
                        <span >个人信息</span>
                    </header>
                    <section >
                        <div className={styles.main}>

                            <Form onSubmit={handleSubmit}>
                                <FormItem>
                                    <div className={styles.xinxi} >姓名：</div>
                                    {getFieldDecorator('Name', {
                                        rules: [{ required: true, message: '请输入姓名名！' }]
                                    })(
                                        <Input
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >班级：</div>
                                    {getFieldDecorator('banji', {
                                        rules: [{ required: true, message: '请输入班级！' }]
                                    })(
                                        <Input
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >学号：</div>
                                    {getFieldDecorator('xuehao', {
                                        rules: [{ required: true, message: '请输入学号！' }]
                                    })(
                                        <Input
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >学院：</div>
                                    {getFieldDecorator('xueyua', {
                                        rules: [{ required: true, message: '请输入学院！' }]
                                    })(
                                        <Input
                                        />
                                    )}
                                </FormItem>

                                <Button type="primary" htmlType="submit" className={styles.contentl} >
                                    注册
					</Button>
                            </Form>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    );
}
Renyuas = connect((state) => {
    return {
        ...state.renyua,
    };
})(Renyuas);

export default create()(Renyuas);