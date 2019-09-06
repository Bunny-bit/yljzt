import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker, Select } from 'antd';
import ImageUpload from '../../components/ImageUpload/ImageUpload';
import { remoteUrl } from '../../utils/url';

const create = Form.create;
const FormItem = Form.Item;

const Option = Select.Option;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

function AppStartPage({ form, record }) {
	const columns = [
		{
			title: '系统',
			dataIndex: 'platform',
			sorter: true,
			render: (text, record) => (text == 1 ? <span>Android</span> : <span>IOS</span>)
		},
		{
			title: '图片',
			dataIndex: 'url',
			sorter: true,
			render: (text, record) => (
				<div>
					<img style={{ height: '60px' }} src={`${remoteUrl}${text}`} />
				</div>
			)
		},
		{
			title: '宽度',
			dataIndex: 'width_Px',
			sorter: true
		},
		{
			title: '高度',
			dataIndex: 'high_Px',
			sorter: true
		},
		{
			title: '添加时间',
			dataIndex: 'creationTime',
			sorter: true,
			render: (text, record) => (text ? <span>{moment(text).format('YYYY-MM-DD HH:mm')}</span> : null)
		},
		{
			title: '最后一次修改时间',
			dataIndex: 'lastModificationTime',
			sorter: true,
			render: (text, record) => (text ? <span>{moment(text).format('YYYY-MM-DD HH:mm')}</span> : null)
		}
	];
	const { getFieldDecorator } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const formNode = (
		<Form>
			<FormItem label="系统" {...formCol}>
				{getFieldDecorator('platform', {
					initialValue: record.platform,
					rules: [ { required: true, message: '请填选择系统' } ]
				})(
					<Select>
						<Option value="1">Android</Option>
						<Option value="2">IOS</Option>
					</Select>
				)}
			</FormItem>
			<FormItem label="图片" {...formCol}>
				{getFieldDecorator('url', {
					initialValue: record.url,
					rules: [ { required: true, message: '请上传图片' } ]
				})(<ImageUpload />)}
			</FormItem>
			<FormItem label="宽度" {...formCol}>
				{getFieldDecorator('width_Px', {
					initialValue: record.width_Px
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="高度" {...formCol}>
				{getFieldDecorator('high_Px', {
					initialValue: record.high_Px
				})(<InputNumber />)}
			</FormItem>
		</Form>
	);
	const filters = [
		{
			name: 'creationTime',
			displayName: '添加时间',
			type: 'datetime',
			option: '>='
		},
		{
			name: 'creationTime',
			displayName: '',
			type: 'datetime',
			option: '<'
		}
	];

	return (
		<CRUD
			form={form}
			getAllApi={new api.AppStartPageApi().appAppStartPageGetAll}
			deleteApi={new api.AppStartPageApi().appAppStartPageDelete}
			updateApi={new api.AppStartPageApi().appAppStartPageUpdate}
			createApi={new api.AppStartPageApi().appAppStartPageCreate}
			deleteBatchApi={new api.AppStartPageApi().appAppStartPageDeleteBatch}
			columns={columns}
			formNode={formNode}
			filterProps={{
				filters,
				searchProvide: 'sql'
			}}
		/>
	);
}

AppStartPage = connect((state) => {
	return {
		...state.crud
	};
})(AppStartPage);
export default create()(AppStartPage);
