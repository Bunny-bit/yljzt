import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Modal } from 'antd';
import { remoteUrl } from '../../utils/url';
import styles from './ServerImageBrowse.css';

class ServerImageBrowse extends React.Component {
	componentWillReceiveProps(nextProps) {
		if (this.props.visible == false && nextProps.visible == true) {
			this.props.dispatch({
				type: 'serverImageBrowse/getAll',
				payload: { start: 0 }
			});
		}
	}
	handleClick = (url) => {
		this.props.onChange(url);
	};
	render() {
		const { dispatch, visible, total, items } = this.props;
		return (
			<Modal visible={visible} onCancel={this.props.onCancel} footer={null} width={540}>
				<div className={styles.imageList}>
					<ul className={styles.ul}>
						{items.map((n) => (
							<li key={n.url} className={styles.li} onClick={this.handleClick.bind(this, n.url)}>
								<img width="113" height="113" src={remoteUrl + n.url} />
								<span
									className={styles.icon}
									style={n.selectUrl == n.url ? { border: '5px solid #1094fa' } : null}
								/>
							</li>
						))}
					</ul>
				</div>
			</Modal>
		);
	}
}
ServerImageBrowse = connect((state) => {
	return {
		...state.serverImageBrowse
	};
})(ServerImageBrowse);

export default ServerImageBrowse;
