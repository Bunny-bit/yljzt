import React from 'react';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Modal, Tooltip, Table, DatePicker } from 'antd';
const create = Form.create;
const FormItem = Form.Item;
import moment from 'moment';
const RangePicker = DatePicker.RangePicker;

import { connect } from 'dva';

function UserLogin({ dispatch, loading, visible, pagination, items, form }) {
	const { getFieldDecorator } = form;

	function handleCancel() {
		dispatch({
			type: 'userLogin/setState',
			payload: {
				visible: !visible
			}
		});
	}

	const columns = [
		{
			title: '登录状态',
			dataIndex: 'exception',
			render: (text, record) => (
				<center>
					{record.result == 'Success' ? (
						<Badge status="success" text="成功" />
					) : (
						<Badge status="default" text="失败" />
					)}
				</center>
			)
		},
		{
			title: '时间',
			dataIndex: 'creationTime',
			sorter: true
		},
		{
			title: '用户名',
			dataIndex: 'userNameOrEmailAddress',
			sorter: true
		},
		{
			title: '登录IP地址',
			dataIndex: 'clientIpAddress',
			sorter: true
		},
		{
			title: '客户端',
			dataIndex: 'clientName',
			sorter: true
		},
		{
			title: '浏览器',
			dataIndex: 'browserInfo',
			sorter: true
		}
	];

	function _onChange(pagination, filters, sorter) {
		dispatch({
			type: 'userLogin/getUserLogins',
			payload: {
				...pagination,
				...sorter
			}
		});
	}

	// rowSelection object indicates the need for row selection
	const rowSelection = {
		onChange: (selectedRowKeys, selectedRows) => {
			console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
		}
	};

	return (
		<Modal
			visible={visible}
			width={900}
			title="查看登录历史"
			onCancel={handleCancel}
			footer={[
				<Button key="submit" loading={loading} onClick={handleCancel}>
					关闭
				</Button>
			]}
		>
			<div>
				<Form layout="inline" className="ant-advanced-search-form">
					<Row gutter={16}>
						<FormItem label="日期范围">
							{getFieldDecorator('dateRange', {
								initialValue: [ moment().subtract(3, 'days'), moment() ]
							})(<RangePicker style={{ width: '100%' }} format="YYYY-MM-DD" />)}
						</FormItem>
						<Button
							type="primary"
							onClick={() => {
								form.validateFields((err, values) => {
									if (!err) {
										var payload = {
											...pagination,
											...values
										};
										dispatch({
											type: 'userLogin/getUserLogins',
											payload: payload
										});
									}
								});
							}}
						>
							查询
						</Button>
					</Row>
				</Form>

				<Table
					columns={columns}
					dataSource={items}
					pagination={pagination}
					loading={loading}
					onChange={_onChange}
					rowKey={(record) => record.creationTime}
					bordered={true}
					size="middle"
				/>
			</div>
		</Modal>
	);
}

UserLogin = connect((state) => {
	return {
		...state.userLogin
	};
})(UserLogin);

export default create()(UserLogin);
