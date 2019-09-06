import React from 'react';
import styles from './Editor.css';
import { Form, Button, Input } from 'antd';
const Item = Form.Item;
const create = Form.create;

import { connect } from 'dva';
import Ueditor from './../../components/Ueditor/Ueditor';

/**
 * Editor.js
 * Created by 李廷旭 on 2017/9/26 10:49
 * 描述: 富文本编辑器Demo
 */
function Editor({ dispatch, form }) {
	function _submit(e) {
		e.preventDefault();
		form.validateFields((err, values) => {
			console.log(values);
			// getFieldDecorator('view', { initialValue: '预览文章...' });
			// form.setFieldsValue({
			// 	view: values.article
			// });
		});
	}

	const formCol = {
		labelCol: { span: 2 },
		wrapperCol: { span: 22 }
	};

	const { getFieldDecorator, getFieldValue } = form;
	getFieldDecorator('view', { initialValue: '预览文章...' });
	const view = getFieldValue('view');
	return (
		<div>
			<div className={styles.editor}>
				<Form onSubmit={_submit}>
					<Item label="文章标题" {...formCol}>
						{getFieldDecorator('title', {
							initialValue: ''
						})(<Input placeholder="请输入" />)}
					</Item>
					<Item label="文章内容" {...formCol}>
						{getFieldDecorator('article', {
							initialValue: '初始化内容'
						})(<Ueditor height={500} />)}
					</Item>
					<Item label="文章预览" {...formCol}>
						<div className={styles.art} dangerouslySetInnerHTML={{ __html: getFieldValue('article') }} />
					</Item>
					<Item wrapperCol={{ span: 22, offset: 2 }}>
						<Button type="primary" htmlType="submit">
							提交
						</Button>
					</Item>
				</Form>
			</div>
		</div>
	);
}

Editor = connect()(Editor);

export default create()(Editor);
