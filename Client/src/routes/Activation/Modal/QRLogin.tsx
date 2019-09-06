import React from 'react';
import { connect } from 'dva';
import { Link } from 'dva/router';
import { Form, Icon, Tooltip, Spin } from 'antd';
import QRCode from 'qrcode.react';
import styles from './Activation.css';
const create = Form.create;
function QRLogin({ dispatch, form, url, scanQRCode }) {
	return (
		<div style={{ position: 'relative' }}>
			<div style={{ position: 'absolute', right: '0' }}>
				<Tooltip placement="left" title="账号密码登录">
					<Link to="/">
						<Icon type="desktop" style={{ fontSize: 30, color: '#08c' }} />
					</Link>
				</Tooltip>
			</div>
			<div className={styles.headcol}>手机扫码，安全登录</div>
			<div style={{ textAlign: 'center', padding: '50px 0' }}>
				{scanQRCode ? (
					<div
						style={{
							height: '200px',
							lineHeight: '200px',
							position: 'absolute',
							width: '100%',
							backgroundColor: 'rgba(0,0,0,0.75)',
							color: 'white',
							fontSize: '25px'
						}}
					>
						扫码成功，请确认登录
					</div>
				) : null}
				{url ? (
					<QRCode value={url} size={200} />
				) : (
					<div style={{ height: '200px', lineHeight: '200px' }}>加载中。。。</div>
				)}
			</div>
		</div>
	);
}
QRLogin = connect((state) => {
	return {
		...state.qrLogin
	};
})(QRLogin);

export default create()(QRLogin);
