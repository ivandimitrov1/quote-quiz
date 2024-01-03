import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';
import { useNavigate } from 'react-router-dom';

const QuizList = () => {
  const { token } = useAuth();
  const navigate = useNavigate();

  const [myList, setMyList] = useState([]);
  const [quizes, setQuizes] = useState([]);

  const [feedback, setFeedback] = useState(null);
  const successStyle = "alert alert-success";
  const errorStyle = "alert alert-danger";

  const fetchQuizes = async () => {
    try {
      const response = await fetch(`https://localhost:7197/api/quizes/list`, {
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
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const fetchMyQuizes = async () => {
    try {
      const response = await fetch(`https://localhost:7197/api/quizes/mylist`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const data = await response.json();
      setMyList(data);
    } catch (error) {
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const resumeQuiz = (quizId) => {
    if (myList.includes(quizId)) {
      return true;
    }

    return false;
  };

  const goToQuizStart = (quizId) => {
    let resume = resumeQuiz(quizId) ? "resume" : "start";
    navigate('/quiz-start/' + quizId + '/' + resume);
  };

  useEffect(() => {
    fetchMyQuizes();
    fetchQuizes();
  }, []);


  return (
      <div>
        <div className="container mt-4">
          <h2 className="mb-4">Quiz List</h2>
          {feedback &&
            <div class={feedback.style} role="alert">
              {feedback.message}
            </div>
          }
          <table className="table table-hover">
            <thead className="thead-dark">
              <tr>
                <th> ID </th>
                <th> Title </th>
                <th> Quote Count </th>
                <th> Actions </th>
              </tr>
            </thead>
            <tbody>
              {quizes.map(quiz => (
                <tr key={quiz.id}>
                  <td>{quiz.id}</td>
                  <td>{quiz.title}</td>
                  <td>{quiz.quoteCount}</td>
                  <td>
                    <button onClick={() => goToQuizStart(quiz.id)}> {resumeQuiz(quiz.id) ? "Resume" : "Start"}</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div>
        </div>
      </div>
  );
}

export default QuizList;