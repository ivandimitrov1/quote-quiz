import React, { useState } from 'react';

import { useAuth } from './auth.context';

const LoginForm = () => {
  const { setToken } = useAuth();

  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch('https://localhost:7197/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
        body: JSON.stringify({ login, password }),
      });

      if (!response.ok) {
        const result = await response.json();
        setError(result.detail || JSON.stringify(result.errors));
      }

      const { token } = await response.json();
      setToken(token);
    } catch (error) {
      console.error('Login failed:', error.message);
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <div className="card">
            <div className="card-body">
              <form onSubmit={handleSubmit}>
                <h3 className="text-center">Login</h3>

                {error &&
                  <div class="alert alert-danger" role="alert">
                    {error}
                  </div>
                }

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-2">
                      <span class="input-group-text" id="inputGroup-sizing-default">Username</span>
                    </div>
                    <input type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"
                      placeholder="Enter username" value={login} onChange={(e) => setLogin(e.target.value)} />
                  </div>
                </div>

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-2">
                      <span class="input-group-text" id="inputGroup-sizing-default">Password</span>
                    </div>
                    <input type="password" class="form-control" aria-label="Default"
                      aria-describedby="inputGroup-sizing-default" placeholder="Enter password"
                      value={password} onChange={(e) => setPassword(e.target.value)} />
                  </div>
                </div>

                <button type="submit" className="btn btn-primary btn-block">Submit</button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginForm;