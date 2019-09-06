import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, Row, Col, Icon, Badge, Modal, Button } from 'antd';
import styles from './AuditLog.css';

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

class AuditLog extends React.Component {
	state = {
		visible: false,
		record: {}
	};
	render() {
		const { dispatch, form } = this.props;
		const { visible, record } = this.state;
		const columns = [
			{
				title: '状态',
				dataIndex: 'exception',
				render: (text, record) => (
					<center>
						{record.exception ? <Badge status="default" text="错误" /> : <Badge status="success" text="成功" />}
					</center>
				)
			},
			{
				title: '时间',
				dataIndex: 'executionTime',
				sorter: true
			},
			{
				title: '用户名',
				dataIndex: 'userName',
				sorter: true
			},
			{
				title: 'Service',
				dataIndex: 'serviceName',
				sorter: true
			},
			{
				title: 'Action',
				dataIndex: 'methodName',
				sorter: true
			},
			{
				title: '相应时长',
				dataIndex: 'executionDuration',
				sorter: true,
				render: (text, record) => `${text}ms`
			},
			{
				title: 'IP',
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
			},
			{
				title: '操作',
				dataIndex: 'option',
				width: 80,
				fixed: 'right',
				render: (text, record) => (
					<center>
						<a
							type="primary"
							onClick={() => {
								this.setState({
									visible: true,
									record: record
								});
							}}
						>
							查看详情
						</a>
					</center>
				)
			}
		];
		const { getFieldDecorator } = form;
		const formCol = {
			labelCol: { span: 3 },
			wrapperCol: { span: 21 }
		};

		const handleCancel = () => {
			this.setState({
				visible: false
			});
		};

		function getFormattedParameters(parameters) {
			try {
				var json = JSON.parse(parameters);
				return JSON.stringify(json, null, 4);
			} catch (e) {
				return parameters;
			}
		}

		return (
			<div>
				<CRUD
					form={form}
					modalWidth={800}
					getAllApi={new api.AuditLogApi().appAuditLogGetAuditLogs}
					toExcelApi={new api.AuditLogApi().appAuditLogGetAuditLogsToExcel}
					columns={columns}
					filterProps={{
						filters: [
							{
								name: 'startDate',
								displayName: '开始时间',
								type: 'datetime',
								value: moment().subtract(3, 'days')
							},
							{
								name: 'endDate',
								displayName: '结束时间',
								type: 'datetime',
								format: 'YYYY-MM-DD 23:59:59',
								value: moment()
							}
						],
						advancedFilters: [
							{
								name: 'startDate',
								displayName: '开始时间',
								type: 'datetime',
								value: moment().subtract(3, 'days')
							},
							{
								name: 'endDate',
								displayName: '结束时间',
								type: 'datetime',
								format: 'YYYY-MM-DD 23:59:59',
								value: moment()
							},
							{
								name: 'userName',
								displayName: '用户名'
							},
							{
								name: 'minExecutionDuration',
								displayName: '相应时长从',
								type: 'number'
							},
							{
								name: 'maxExecutionDuration',
								displayName: '到',
								type: 'number'
							},
							{
								name: 'serviceName',
								displayName: 'Service'
							},
							{
								name: 'methodName',
								displayName: 'Action'
							},
							{
								name: 'browserInfo',
								displayName: '浏览器'
							},
							{
								name: 'hasException',
								displayName: '错误状态',
								type: 'select',
								selectOptions: [
									{ name: '全部', value: '' },
									{ name: '成功', value: 'false' },
									{ name: '出现错误', value: 'true' }
								]
							}
						]
					}}
				/>
				<Modal
					visible={visible}
					width={900}
					title="系统日志详情"
					onCancel={handleCancel}
					footer={[
						<Button key="submit" onClick={handleCancel}>
							取消
						</Button>
					]}
				>
					<div>
						<Row type="flex" gutter={12}>
							<Col span={12} className={styles.row}>
								<h2>用户信息</h2>
								<Row gutter={24} type="flex">
									<Col span={4}>用户名:</Col>
									<Col span={20}>{record.userName}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>客户端:</Col>
									<Col span={20}>{record.clientName}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>IP地址:</Col>
									<Col span={20}>{record.clientIpAddress}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>浏览器:</Col>
									<Col span={20}>{record.browserInfo}</Col>
								</Row>
								<h2>自定义数据</h2>
								<Row gutter={24} type="flex">
									{record.customData == null ? (
										<Col span={24}>没有自定义数据</Col>
									) : (
										<Col span={24}>
											<pre>{record.customData}</pre>
										</Col>
									)}
								</Row>
							</Col>
							<Col span={12} className={styles.row}>
								<h2>操作信息</h2>
								<Row gutter={24} type="flex">
									<Col span={4}>Service:</Col>
									<Col span={20}>{record.serviceName}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>Action:</Col>
									<Col span={20}>{record.methodName}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>操作时间:</Col>
									<Col span={20}>{record.executionTime}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>相应时长:</Col>
									<Col span={20}>{record.executionDuration + 'ms'}</Col>
								</Row>
								<Row gutter={24} type="flex">
									<Col span={4}>参数:</Col>
									<Col span={20}>
										<pre long="js">{getFormattedParameters(record.parameters)}</pre>
									</Col>
								</Row>
							</Col>
							<Col span={24} className={styles.row}>
								<h2>错误状态</h2>
								<Row gutter={24} type="flex">
									{record.exception == null ? (
										<Col span={24}>成功</Col>
									) : (
										<Col span={24}>
											<pre>{record.exception}</pre>
										</Col>
									)}
								</Row>
							</Col>
						</Row>
					</div>
				</Modal>
			</div>
		);
	}
}

AuditLog = connect((state) => {
	return {
		...state.crud
	};
})(AuditLog);
export default create()(AuditLog);
