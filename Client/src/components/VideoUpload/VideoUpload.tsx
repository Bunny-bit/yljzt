import React from 'react';
import { Upload, message, Button, Icon, Modal } from 'antd';
import { remoteUrl } from './../../utils/url';

class VideoUpload extends React.Component {
	static defaultProps = {
		headersAuthorization: 'Bearer ' + window.localStorage.getItem('token'),
		action: `${remoteUrl}/ImageFile/UploadVideoPicture`,
		typeFilter: [ 'video/mp4' ],
		typeErrorText: '请上传MP4类型的文件'
	};
	state = {
		previewVisible: false
	};
	handleCancel = () => this.setState({ previewVisible: false });
	handlePreview = () => {
		this.setState({
			previewVisible: true
		});
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
			beforeUpload(file) {
				if (thisProps.typeFilter && thisProps.typeFilter.length > 0) {
					var isTypeTrue = thisProps.typeFilter.indexOf(file.type) >= 0;
					if (!isTypeTrue) {
						message.warning(thisProps.typeErrorText);
					}
					return isTypeTrue;
				}
			},
			onChange(info) {
				if (info.file.status === 'done') {
					if (info.fileList[info.fileList.length - 1].response.error) {
						message.error('上传失败 ' + info.fileList[info.fileList.length - 1].response.error.message);
						return;
					}
					thisProps.onChange(info.fileList[info.fileList.length - 1].response.result.url);
					message.success('上传成功');
				} else if (info.file.status === 'error') {
					message.error('上传失败');
				}
				if (info.fileList.length > 1) {
					info.fileList.shift();
				}
			}
		};
		return this.props.value ? (
			<div>
				<Button.Group>
					<Button onClick={this.handlePreview}>
						<Icon type="eye-o" /> 预览
					</Button>
					<Upload {...props}>
						<Button>
							<Icon type="edit" /> 重新上传
						</Button>
					</Upload>
				</Button.Group>

				<Modal visible={this.state.previewVisible} footer={null} onCancel={this.handleCancel}>
					{this.state.previewVisible ? (
						<video controls="controls" autoplay="autoplay" width="100%">
							<source src={remoteUrl + this.props.value} type="video/mp4" />
							您的浏览器不支持 video 标签。
						</video>
					) : null}
				</Modal>
			</div>
		) : (
			<Upload {...props}>
				<Button>
					<Icon type="upload" /> 上传视频
				</Button>
			</Upload>
		);
	}
}

export default VideoUpload;
