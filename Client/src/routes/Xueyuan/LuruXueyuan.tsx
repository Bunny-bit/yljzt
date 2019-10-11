
import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import styles from './Luruxueyuan.css';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
const FormItem = Form.Item;
const create = Form.create;


function LuruXueyuan({ dispatch, form }) {
	const { getFieldDecorator } = form;

	function handleSubmit(e) {
		form.validateFields((err, values) => {
			console.log(values)
			let data = {
				"xueyuan": values.xueyuan
			};
			if (!err) {
				dispatch({
					type: 'xueyuan/creatXueyuan',
					payload: data
				});
			}
		})
	}

	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	return (
		<div className={styles.container}>
			<div className={styles.content}>

				<section>
					<Form onSubmit={handleSubmit} >
						<div>学院</div>
						<FormItem label="名称"{...formCol}>
							<span>名称</span>
							{getFieldDecorator('xueyuan', {
								rules: [{ required: true, message: '请输入名称！' }]
							})(
								<Input />
							)}
						</FormItem>



						<Button type="primary" htmlType="submit">
							登录
					</Button>
					</Form>

				</section>
			</div>
		</div>
	);
}

LuruXueyuan = connect((state) => {
	return {
		...state.xueyuan,
	};
})(LuruXueyuan);

export default create()(LuruXueyuan);