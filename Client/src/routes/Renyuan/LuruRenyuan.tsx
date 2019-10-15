import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
const FormItem = Form.Item;
const create = Form.create;
import styles from './LuruRenyuan.css';

function LuruRenyuan({ dispatch, form,}){
    const {getFieldDecorator}=form;
    function handleSubmit(e: any){
        form.validateFields((err: any, values: { xingming: any; banji: any; xueyuan: any; }) => {
            console.log(values);
        let data = {
                "id": 0,
                "xingming":values.xingming,
                "banji": values.banji,
                "xuehao": 0,
                "xueyuan":values.xueyuan
            };
            if (!err) {
                dispatch({
                    type: 'renyuan/creatrenyuan',
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
            <div className={styles.container}>
                <div className={styles.content}>
                <header className={styles.xingxi}>
                    <span>个人信息</span>
                </header>
                <section>
                <Form onSubmit={handleSubmit}>
                    <FormItem  className={styles.xingming}>
                    <span>姓名</span>
                        {getFieldDecorator('xingming', {
                            rules: [ { required: true, message: '请输入姓名！' } ]
                        })(
                            <Input/>
                        )}
                    </FormItem>
                    <FormItem className={styles.xingming}>
                    <span>班级</span>
                        {getFieldDecorator('banji', {
                            rules: [ { required: true, message: '请输入班级！' } ]
                        })(
                            <Input/>
                        )}
                    </FormItem>
                    <FormItem className={styles.xingming}>
                    <span>学号</span>
                            {getFieldDecorator('xuehao', {
                                rules: [ { required: true, message: '请输入学号！' } ]
                            })(
                                <Input/>
                            )}
                    </FormItem>
                    <FormItem className={styles.xingming}>
                    <span>学院</span>
                        {getFieldDecorator('xueyuan', {
                            rules: [ { required: true, message: '请输入学院！' } ]
                        })(
                                <Input/>
                        )}
                    </FormItem>
                    <FormItem className={styles.tijiao}>
                    <Button type="primary" htmlType="submit" className={styles.login}>
                        提交
                    </Button>
                </FormItem>
                </Form>
            </section>
        </div>
    </div>
)};
LuruRenyuan = connect((state: { renyuan: any; }) => {
	return {
        ...state.renyuan;
	};
})(LuruRenyuan);

export default create()(LuruRenyuan);