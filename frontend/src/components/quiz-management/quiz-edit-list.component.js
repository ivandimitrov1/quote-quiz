import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';

const QuizEditList = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [quizes, setQuizes] = useState([]);

  const [feedbackMessage, setFeedbackMessage] = useState(null);
  const successStyle = "alert alert-success";
  const errorStyle = "alert alert-danger";

  const fetchQuizes = async () => {
    try {
      const response = await fetch(`https://localhost:7197/api/quizmanagement/list`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const data = await response.json();

      setQuizes(data);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const publishQuiz = async (quizId, publish) => {
    const publishApi = publish ? `https://localhost:7197/api/quizmanagement/${quizId}/publish` : `https://localhost:7197/api/quizmanagement/${quizId}/unpublish`;
    try {
      const response = await fetch(publishApi, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      if (response.ok) {
        setFeedbackMessage({ style: successStyle, message: "Successfull operation" });
        fetchQuizes();
      } else {
        var result = await response.json();
        setFeedbackMessage({ style: errorStyle, message: result.detail });
      }
    } catch (error) {
      setFeedbackMessage({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const goToEditForm = (quizId) => {
    navigate('/quiz-edit-form/' + quizId);
  };

  useEffect(() => {
    fetchQuizes();
  }, []);


  return (
    <div className="container mt-4">
      <h2 className="mb-4">Quiz Management&nbsp;&nbsp;
        <Link to="/quiz-edit-form/0" >
          <button type="button" className="btn btn-primary">
            Add Quiz
          </button>
        </Link>
      </h2>

      {feedbackMessage &&
        <div class={feedbackMessage.style} role="alert">
          {feedbackMessage.message}
        </div>
      }

      <table className="table table-hover">
        <thead className="thead-dark">
          <tr>
            <th> ID </th>
            <th> Title </th>
            <th> Published </th>
            <th> Actions </th>
          </tr>
        </thead>
        <tbody>
          {quizes.map(quiz => (
            <tr key={quiz.id}>
              <td>{quiz.id}</td>
              <td>{quiz.title}</td>
              <td>{quiz.published ? "true" : "false"}</td>
              <td>
                <button onClick={() => goToEditForm(quiz.id)}>Edit</button>
                {quiz.published ?
                  <button onClick={() => publishQuiz(quiz.id, false)}>Unpublish</button> :
                  <button onClick={() => publishQuiz(quiz.id, true)}>Publish</button>
                }
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>

  );
}

export default QuizEditList;