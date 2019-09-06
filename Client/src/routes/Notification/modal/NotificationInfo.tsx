import React from 'react';
import styles from './../Notification.css';
import { Form, Checkbox, Input, Button, Row, Col, Icon, Badge, Modal, Tooltip } from 'antd';
const create = Form.create;
const Search = Input.Search;
import moment from 'moment';

import { connect } from 'dva';

function NotificationInfo({ dispatch, expand, form, loading, visible, record }) {
	function handleCancel() {
		dispatch({
			type: 'notification/setState',
			payload: {
				visible: !visible
			}
		});
	}

	return (
		<Modal
			visible={visible}
			width={900}
			title="通知详情"
			onCancel={handleCancel}
			footer={[
				<Button key="submit" loading={loading} onClick={handleCancel}>
					关闭
				</Button>
			]}
		>
			<div>
				<Row type="flex" gutter={12}>
					<Col span={24} className={styles.row3}>
						<h2>基础信息</h2>
						<Row gutter={24} type="flex">
							<Col span={4}>阅读状态:</Col>
							<Col span={20}>{record.state == 0 ? '未读' : '已读'}</Col>
						</Row>
						<Row gutter={24} type="flex">
							<Col span={4}>时间:</Col>
							<Col span={20}>{moment(record.notification.creationTime).format('YYYY-MM-DD HH:mm')}</Col>
						</Row>
						<Row gutter={24} type="flex">
							<Col span={4}>发送人:</Col>
							<Col span={20}>系统发送</Col>
						</Row>
						<Row gutter={24} type="flex">
							<Col span={4}>标题:</Col>
							<Col span={20}>
								{record.notification.data.properties.message ? (
									record.notification.data.properties.message
								) : (
									'系统通知'
								)}
							</Col>
						</Row>
						<h2>通知详情</h2>
						<Row gutter={24} type="flex">
							<Col span={24}>
								{record.notification.data.properties.content ? (
									record.notification.data.properties.content
								) : (
									record.notification.data.properties.message
								)}
							</Col>
						</Row>
					</Col>
				</Row>
			</div>
		</Modal>
	);
}

NotificationInfo = connect((state) => {
	return {
		...state.notification
	};
})(NotificationInfo);

export default create()(NotificationInfo);
