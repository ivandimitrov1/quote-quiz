// UserContext.js
import { createContext, useContext, useState, useEffect } from 'react';

const UserContext = createContext();

export const AuthContext = ({ children }) => {
  const [token, setToken] = useState('');

  useEffect(() => {
    const cookieExists = document.cookie.split(';').some((cookie) => {
      const [name] = cookie.split('=');
      return name.trim() === 'quote-quiz-refresh-token-exist';
    });

    if (cookieExists && !token) {
      setAccessTokenThroughRefreshToken();
    } else {
      setToken(null);
    }
  }, []);


  // REFRESH TOKEN API
  const setAccessTokenThroughRefreshToken = async (e) => {
    try {
      const response = await fetch('https://localhost:7197/api/auth/refreshToken', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
      });

      if (!response.ok) {
        throw new Error('Invalid credentials');
      }

      const { token } = await response.json();
      setToken(token);
    } catch (error) {
      console.log('The cookie does not exist.');
    }
  };

  // LOGOUT API
  const logout = async () => {
    try {
      const response = await fetch('https://localhost:7197/api/auth/logout', {
        method: 'POST',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (response.ok) {
        setToken(null);
      } else {
        // Handle the case where the server returns an error
        console.error('Logout failed', response.statusText);
      }
    } catch (error) {
      console.error('Logout failed', error);
    }
  };

  const username = () => {
    return parseJwt(token).Login;
  }

  const getRole = () => {
    const role = parseJwt(token).Role;
    return {
      isAdmin: role === "Admin",
      isUser: role === "User",
      Readonly: role === "UserReadOnly"
    };
  }

  const parseJwt = (token) => {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  }

  return (
    <UserContext.Provider value={{ token, setToken, logout, username, getRole }}>
      {children}
    </UserContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(UserContext);
};