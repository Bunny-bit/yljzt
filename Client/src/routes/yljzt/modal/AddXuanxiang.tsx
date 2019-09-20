import React from 'react';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Tabs, Modal, Tooltip, Tree } from 'antd';
const create = Form.create;
const FormItem = Form.Item;

import { connect } from 'dva';

function AddXuanxiang({ dispatch, form, visible,currentTimuId }) {
	function handleCancel() {
		dispatch({
			type: 'yljzt/setState',
			payload: {
				visible: false
			}
		});
	}

	const { getFieldDecorator, getFieldValue } = form;
	return (
		<Modal
			visible={visible}
			width={658}
			title="增加选项"
			onCancel={handleCancel}
			footer={[
				<Button
					key="back"
					type="primary"
					htmlType="submit"
					onClick={() => {
						form.validateFields((err, values) => {
							dispatch({
								type: 'yljzt/createXuanxiang',
								payload: {
									timuId:currentTimuId,
									name: values.name,
									neirong: values.neirong
								}
							});

							form.resetFields();
						});
					}}
				>
					提交
				</Button>,
				<Button key="submit" onClick={handleCancel}>
					取消
				</Button>
			]}
		>
			<Form>
				<FormItem label="选项编号：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
					{getFieldDecorator('name', {
						initialValue: '',
						rules: [ { required: true, message: '请填写选项编号' } ]
					})(<Input />)}
				</FormItem>
				<FormItem label="选项内容：" labelCol={{ span: 6 }} wrapperCol={{ span: 15 }}>
					{getFieldDecorator('neirong', {
						initialValue: '',
						rules: [ { required: true, message: '请填写选项内容' } ]
					})(<Input />)}
				</FormItem>
			</Form>
		</Modal>
	);
}

AddXuanxiang = connect((state) => {
	return {
		...state.yljzt
	};
})(AddXuanxiang);

export default create()(AddXuanxiang);
