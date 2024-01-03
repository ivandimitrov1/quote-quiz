import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { AuthContext } from './components/auth/auth.context';
import 'bootstrap/dist/css/bootstrap.min.css';

ReactDOM.render(
  <React.StrictMode>
    <AuthContext>
        <App />
    </AuthContext>
  </React.StrictMode>,
  document.getElementById('root')
);