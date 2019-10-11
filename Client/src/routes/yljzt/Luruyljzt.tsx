import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
const FormItem = Form.Item;
const create = Form.create;



function Luruyljzt({ dispatch, form }) {
    const { getFieldDecorator } = form;
    function handleSubmit(e) {
        form.validateFields((err, values) => {
            let data = {
                gerenXingming: { gerenXingming: values.gerenXingming, isDefault: values.isDefault },
                grantedPermissionNames: selectRoles
            };
            if (!err) {
                dispatch({
                    type: 'role/createOrUpdateRole',
                    payload: data
                });
            }
        }); 

    }
    return (
        <div>
            <div>
                <header>
                    <span>答题列表</span>
                </header>
                <section>
                <Form onSubmit={handleSubmit}>
                    <div>个人信息</div>


                    <FormItem>
                        <span>姓名：</span>
                        {getFieldDecorator('gerenXingming', {
                            rules: [{ required: true, message: '请输入姓名！' }]
                        })(
                            <Input />
                        )}
                    </FormItem>

                    <FormItem>
                        <span>班级：</span>
                        {getFieldDecorator('gerenBanji', {
                            rules: [{ required: true, message: '请输出班级！' }]
                        })(
                            <Input />
                        )}
                    </FormItem>

                    <FormItem>
                        <span>学号：</span>
                        {getFieldDecorator('gerenXuehao', {
                            rules: [{ required: true, message: '请输出学号！' }]
                        })(
                            <Input />
                        )}
                    </FormItem>
                    <Button type="primary" htmlType="submit" >
                        提交信息
					</Button>
                </Form>
                </section>
            </div>
        </div>

    );
}

Luruyljzt = connect((state) => {
	return {
		...state.luruyljzt,
		thirdPartyToken: state.indexpage.thirdPartyToken
	};
})(Luruyljzt);

export default create()(Luruyljzt);