import React from 'react';
import SMS from './SMS';
import Email from './Email';
import styles from './Activation.css';
import {connect} from 'dva';

function Active({locationState}) {
  return (
    <div className={styles.normal}>
      {
        (() => {
          if (locationState && locationState.goToCode) {
            switch (locationState.goToCode) {
              case 1: {
                return <SMS goToCode={locationState.goToCode}/>
              }
              case 2: {
                return <Email goToCode={locationState.goToCode}/>
              }
              case 3: {
                return [<SMS goToCode={locationState.goToCode} key="1"/>,
                  <Email goToCode={locationState.goToCode} key="2"/>]
              }
            }
          }
        })()
      }
    </div>
  )
}

export default connect((state) => {
  return {
    ...state.app,
  }
})(Active);
