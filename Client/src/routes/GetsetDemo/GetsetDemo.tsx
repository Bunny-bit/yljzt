import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber } from 'antd';

const create = Form.create;
const FormItem = Form.Item;
import * as api from '../../api/api';

import GetSet from '../GetSet/GetSet';
import moment from 'moment';

import ImageUpload from '../../components/ImageUpload/ImageUpload';
import IconSelector from '../../components/IconSelector/IconSelector';

function GetsetDemo({ form, data }) {
	const { getFieldDecorator } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const formNode = (
		<Form>
			<FormItem label="文件大小" {...formCol}>
				{getFieldDecorator('fileSize', {
					initialValue: data.fileSize
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="图片" {...formCol}>
				{getFieldDecorator('fileExtension1', {
					initialValue: data.fileExtension1
				})(<ImageUpload />)}
			</FormItem>
			<FormItem label="图标" {...formCol}>
				{getFieldDecorator('fileExtension', {
					initialValue: data.fileExtension
				})(<IconSelector />)}
			</FormItem>
		</Form>
	);

	return (
		<GetSet
			form={form}
			getApi={new api.FileSettingDemoApi().appFileSettingDemoGet}
			setApi={new api.FileSettingDemoApi().appFileSettingDemoSet}
			formNode={formNode}
		/>
	);
}

GetsetDemo = connect((state) => {
	return {
		...state.getSet
	};
})(GetsetDemo);
export default create()(GetsetDemo);
