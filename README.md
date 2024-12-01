# GradientBoostRegression

- **Summary**
    This application uses real data from a local business in Tuscaloosa, AL. The data was legally obtained with the owner's consent for the sole purpose of creating this tool. Its primary goal is to help the business owner project future revenues based on historical data. This tool is designed to provide actionable insights that enable better decision-making, such as optimizing inventory, staffing, and marketing strategies.

    Revenue was pulled from the first regular season game of 2018, to the Oklahoma game of 2024 (11/23/2024). Some playoff games were included in order to give predictions for future playoffs, if applicable. 85 Games were surveyed.

    At the time of finishing this application, the idea was to predict Alabama vs Auburn sales at this particular bar/restaurant. The prediction with this current model was $12753.24, with a human evaluated error of +/- $2000. This youtube video can be seen here: https://www.youtube.com/watch?v=PaEly7lvnBo&feature=youtu.be

    During the Alabama vs. Auburn game, the restaurant achieved $12,963 in sales. This result reflects a prediction error of $209.76, which falls well within the human-evaluated error margin of +/- $2000. However, the slight deviation suggests the model may have been slightly overfit to the training data. This issue can be remedied by decreasing the learning rate or reducing the number of epochs during model training. However, adjusting the maxDepth parameter is not ideal in this scenario, as it could overly simplify the model, causing it to miss important interactions between features critical for predicting high-stakes events like rivalry games.

- **Additional testing**
    There is a simple process to test this model with the data provided in the csv data sheet. A user can open the csv file, remove a row from the file (Thus removing the item from memory), and input the data manually in the 'predict next game' section.

---

## Key Features

- **Gradient Boost Regression**:  
  The program employs Gradient Boost Regression, a popular machine learning algorithm for mid-level regression analysis. Gradient Boosting is effective for capturing complex relationships in data while maintaining interpretability.
- **Historical Data Analysis**:  
  The program analyzes past data, including factors such as whether the game is home or away, the opponent team, and the spread of the game.
- **Customizable Predictions**:  
  Users can input details of upcoming games (e.g., home/away status, opponent team, spread) to predict revenue for specific events.

---

## How It Works

1. **Data Input**:  
   The application processes data stored in a CSV file (`gameday-data.csv`) containing game information and past revenues.
2. **Data Preparation**:  
   Data is encoded for machine learning using numerical representations of categorical data (e.g., "home" or "away" status).
3. **Model Training**:  
   A Gradient Boost Regression model is trained on the dataset to learn patterns and relationships between input features (game details) and revenue.
4. **Revenue Prediction**:  
   Users can input details of future games, and the program predicts the expected revenue based on the trained model.

---

## Technical Details

- **Language and Framework**:  
  The program is written in C# and is designed to be run as a console application.
- **Machine Learning**:  
  Implements a custom Gradient Boost Regression model with decision trees as the base learners. The model iteratively reduces error by minimizing residuals in each step.
- **Key Data Inputs**:
  - Home/Away status
  - Opponent team
  - Game spread
- **Expandable Design**:  
  Additional features like weather, team rankings, or other variables can be added to improve prediction accuracy in future versions.

---

## Limitations

1. **Limited Data Scope**:  
   The current dataset includes only a few core inputs, such as home/away status, opponent team, and game spread. While this provides good baseline predictions, additional features (e.g., weather conditions or team performance) could enhance the model.
2. **Potential Overfitting**:  
   Adding too many features without sufficient data could lead to overfitting, where the model becomes overly complex and less generalizable to new data.
3. **Data Sensitivity**:  
   The program’s accuracy relies heavily on the quality of the input data. Missing, incomplete, or inconsistent data can negatively impact predictions.

---

## Future Enhancements

1. **Additional Inputs**:  
   Incorporating more features like weather conditions, team rankings, ticket sales trends, or even crowd attendance could improve accuracy.
2. **Enhanced Model**:  
   Experimenting with other machine learning algorithms, such as Random Forests or XGBoost, to compare performance and accuracy.
3. **User Interface**:  
   Developing a graphical user interface (GUI) or web-based application for easier interaction.
4. **Real-Time Predictions**:  
   Integrating live game data and APIs for real-time revenue predictions.

---

## Background and Motivation

This project was developed with the assistance of a large-language model (LLM) to provide guidance, debug code, and ensure efficient implementation of machine learning concepts. The foundation of the application, however, is rooted in Gradient Boost Regression, a well-known algorithm for mid-level regression analysis.

In earlier versions of this application, additional data such as the date and time of games were included. However, these features did not consistently improve predictions and sometimes added noise to the model, reducing its accuracy. The current version simplifies the input while maintaining effectiveness.

---

## Legal Disclaimer

This application and its data were created and utilized in compliance with local regulations. The data was legally obtained and is used strictly for educational and business purposes. Redistribution of the data or application without the owner’s consent is prohibited.
