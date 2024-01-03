import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';
import { useParams } from 'react-router-dom';

const QuizEditForm = () => {
  const { token } = useAuth();
  const { quizId } = useParams();

  const [feedback, setFeedback] = useState();
  const [formData, setFormData] = useState({
    "id": 0,
    "title": "",
    "quotes": [],
  });

  useEffect(() => {
    if (quizId > 0) {
      fetchQuiz(quizId);
    }
  }, []);

  const handleNewQuiz = () => {
    setFeedback('');
    setFormData({
      "id": 0,
      "title": "",
      "quotes": []
    });
  };

  const fetchQuiz = async (quizId) => {
    try {
      const response = await fetch('https://localhost:7197/api/quizmanagement/' + quizId, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const data = await response.json();
      let position = 0;
      data.quotes.forEach((quote) => {
        quote.position = position++;

        let answerId = 0
        quote.answers.forEach((answer) => {
          answer.id = answerId++;
        });

      });
      setFormData(data);

    } catch (error) {
      setFeedback(error);
    }
  };

  const handleQuoteTextChange = (position, event) => {
    const newQuotes = formData.quotes.map((quote) =>
      quote.position === position ? { ...quote, text: event.target.value } : quote
    );
    setFormData({ ...formData, quotes: newQuotes });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    let method = formData.id === 0 ? 'POST' : 'PUT';
    try {
      const response = await fetch('https://localhost:7197/api/quizmanagement', {
        method: method,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
        body: JSON.stringify(formData),
      });

      if (!response.ok) {
        const result = await response.json();
        setFeedback(result.detail);
      } else {
        const result = await response.json();
        let position = 0;
        result.quotes.forEach((quote) => {
          quote.position = position++;
        });
        setFormData(result);
        setFeedback("Successfull operation.");
      }

    } catch (error) {
      setFeedback("Successfull operation.");
    }
  };

  const handleAddYesOrNoQuote = () => {
    const newQuotes = [
      ...formData.quotes,
      {
        position: formData.quotes.length,
        text: '',
        answers: [{ id: 0, text: '', isCorrect: false }]
      },
    ];
    setFormData({ ...formData, quotes: newQuotes });
  };

  const handleAddTrippleAnswerQuote = () => {
    const newQuotes = [
      ...formData.quotes,
      {
        position: formData.quotes.length,
        text: '',
        answers: [{ id: 0, text: '', isCorrect: false }, { id: 1, text: '', isCorrect: false }, { id: 2, text: '', isCorrect: false }]
      },
    ];
    setFormData({ ...formData, quotes: newQuotes });
  };

  const handleRemoveQuote = (position) => {
    const newQuotes = formData.quotes.filter((quote) => quote.position !== position);
    setFormData({ ...formData, quotes: newQuotes });
  };

  const handleCorrectAnswerChange = (position, answerId) => {
    const newQuotes = formData.quotes.map((quote) => {
      if (quote.position === position) {
        let answers = quote.answers.map(answer => answer.id === answerId ? { ...answer, isCorrect: !answer.isCorrect } : answer);
        return { ...quote, answers: answers }
      } else {
        return quote;
      }
    }
    );
    setFormData({ ...formData, quotes: newQuotes });
  };

  const handleQuoteAnswerChange = (position, answerId, event) => {
    const newQuotes = formData.quotes.map((quote) => {
      if (quote.position === position) {
        let answers = quote.answers.map(answer => answer.id === answerId ? { ...answer, text: event.target.value } : answer);
        return { ...quote, answers: answers }
      } else {
        return quote;
      }
    }
    );
    setFormData({ ...formData, quotes: newQuotes });
  };

  return (
    <div className="container mt-4">
      <form onSubmit={handleSubmit} className="container">
        <h2 className="mb-3">{formData.id === 0 ? "Add" : "Edit"} Quiz</h2>
        {feedback && <div className="alert alert-info">{feedback}</div>}

        <div class="input-group">
          <input type="text" class="form-control" placeholder="Enter quiz name" value={formData.title}
            onChange={(event) => setFormData({ ...formData, title: event.target.value })} />
          <div class="input-group-append">
            <button class="btn btn-outline-secondary" type="button" onClick={handleAddYesOrNoQuote}>Add Yes/No Quote</button>
            <button class="btn btn-outline-secondary" type="button" onClick={handleAddTrippleAnswerQuote}>Add Tripple Answer Quote</button>

            {formData.id === 0 ?
              <button type="submit" className="btn btn-success">Add Quiz</button>
              : <>
                <button type="submit" className="btn btn-primary">Edit Quiz</button>
                <button type="button" className="btn btn-outline-secondary ml-2" onClick={handleNewQuiz}>New Quiz</button>
              </>
            }
          </div>
        </div>
        <br />
        <div>
          {formData.quotes.map((quote, index) => (
            <div key={quote.position} className="p-2">
              <div class="input-group">
                <div class="input-group-prepend">
                  <button class="input-group-text">{quote.position + 1}</button>
                </div>
                <input class="form-control" type="text" placeholder="Enter quote" value={quote.text} onChange={(event) => handleQuoteTextChange(quote.position, event)} ></input>
                <div class="input-group-append">
                  <button type="button" className="btn btn-danger" onClick={() => handleRemoveQuote(quote.position)}>
                    X
                  </button>
                </div>
              </div>

              <div class="row">
                {quote.answers.map((answer) => (
                  <div key={answer.id} class="col-sm">


                    <div class="input-group">
                      <input type="text" placeholder="Enter answer" class="form-control" value={answer.text} onChange={(event) => handleQuoteAnswerChange(quote.position, answer.id, event)} />
                      <div class="input-group-append">
                        <div class="input-group-text">

                          <div class="form-check form-switch">
                            <input class="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckDefault" checked={answer.isCorrect} onChange={() => handleCorrectAnswerChange(quote.position, answer.id)} />
                            <label class="form-check-label" for="flexSwitchCheckDefault">Is Correct ?</label>
                          </div>

                        </div>
                      </div>
                    </div>

                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </form>
    </div>
  );
};

export default QuizEditForm;