import React from 'react';
import { Upload, message, Button, Icon } from 'antd';
import { remoteUrl } from './../../utils/url';

class FormUpload extends React.Component {
	static defaultProps = {
		buttonText: '上传文件',
		deleteText: '删除文件',
		headersAuthorization: 'Bearer ' + window.localStorage.getItem('token'),
		action: `${remoteUrl}/File/UploadFile`
	};
	render() {
		let thisProps = this.props;
		const props = {
			name: 'file',
			action: this.props.action,
			headers: {
				authorization: this.props.headersAuthorization
			},
			showUploadList: { showRemoveIcon: false },
			onChange(info) {
				if (info.file.status === 'done') {
					if (info.fileList[info.fileList.length - 1].response.error) {
						message.error('上传失败 ' + info.fileList[info.fileList.length - 1].response.error.message);
						return;
					}
					if (info.fileList.length > 1) {
						info.fileList.shift();
					}
					thisProps.onChange(info.fileList[info.fileList.length - 1].response.result.url);
					message.success('上传成功');
				} else if (info.file.status === 'error') {
					message.error('上传失败');
				}
			}
		};
		return this.props.value ? (
			<Button onClick={() => this.props.onChange()}>
				<Icon type="delete" /> {this.props.deleteText}
			</Button>
		) : (
			<Upload {...props}>
				<Button>
					<Icon type="upload" /> {this.props.buttonText}
				</Button>
			</Upload>
		);
	}
}

export default FormUpload;
