import React from 'react';
import NextQuote from './next-quote.component';

const MultipleChoiceQuote = ({ quote, answerQuote, getNextQuote, correctAnswer, retryQuiz }) => {

    return (
        <div className="card-body">
            <blockquote className="blockquote text-center">
                <p className="mb-0">{quote.quizTitle}</p>
                <footer className="blockquote-footer mt-2">{quote.quoteText}</footer>
            </blockquote>
            <div className="text-center">
                {correctAnswer ?
                    <div>
                        {
                            correctAnswer.isCorrect ?
                                <div class="alert alert-success" role="alert">Correct</div>
                            : <div class="alert alert-danger" role="alert">Wrong answer. Correct answer is {correctAnswer.correctAnswer}.</div>
                        }

                        <NextQuote quote={quote} getNextQuote={getNextQuote} retryQuiz={retryQuiz} />
                    </div>
                    : 
                    <div>
                        {quote.quoteAnswers.map((answer, index) => (
                            <div><button
                                className="btn btn-primary mb-2"
                                onClick={() => answerQuote(quote.quizId, quote.quoteId, index)}>
                                {answer}
                            </button>
                            </div>
                        ))}
                    </div>
                }
            </div>
        </div>
    );
}

export default MultipleChoiceQuote;