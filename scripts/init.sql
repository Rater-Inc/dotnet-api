CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,                     -- Unique identifier for each user
    nickname VARCHAR(255) NOT NULL,                 -- Nickname of the user
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP  -- Timestamp when the user was created
);

CREATE TABLE spaces (
    space_id SERIAL PRIMARY KEY,                      -- Unique identifier for each space
    creator_id INT NOT NULL,                          -- Reference to the user who created the space
    name VARCHAR(255) NOT NULL,                       -- Name of the space
    description TEXT,                                 -- Description of the space
    is_locked BOOLEAN DEFAULT FALSE,                  -- Indicates if the space is locked
    password VARCHAR(255),                            -- Password for accessing the space
    link VARCHAR(255) NOT NULL,                       -- Link to the space
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Timestamp when the space was created
    FOREIGN KEY (creator_id) REFERENCES users(user_id) -- Foreign key relation to the users table
);

CREATE TABLE metrics (
    metric_id SERIAL PRIMARY KEY,                    -- Unique identifier for each metric
    space_id INT NOT NULL,                           -- Reference to the space this metric belongs to
    name VARCHAR(255) NOT NULL,                      -- Name of the metric
    description TEXT,                                -- Description of the metric
    FOREIGN KEY (space_id) REFERENCES spaces(space_id), -- Foreign key relation to the spaces table
    UNIQUE (space_id, name)                          -- Unique constraint to ensure metric names are unique within the same space    
);

CREATE TABLE participants (
    participant_id SERIAL PRIMARY KEY,              -- Unique identifier for each participant
    space_id INT NOT NULL,                          -- Reference to the space this participant belongs to
    participant_name VARCHAR(255) NOT NULL,         -- Name of the participant
    FOREIGN KEY (space_id) REFERENCES spaces(space_id), -- Foreign key relation to the spaces table
    UNIQUE (space_id, participant_name)             -- Unique constraint to ensure participant names are unique within the same space
);

CREATE TABLE ratings (
    rating_id SERIAL PRIMARY KEY,                   -- Unique identifier for each rating
    rater_id INT NOT NULL,                          -- Reference to the user who is giving the rating
    ratee_id INT NOT NULL,                          -- Reference to the participant who is being rated
    space_id INT NOT NULL,                          -- Reference to the space this rating belongs to
    metric_id INT NOT NULL,                         -- Reference to the metric being rated
    score INT NOT NULL,                             -- Score given for the rating
    rated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Timestamp when the rating was given
    FOREIGN KEY (rater_id) REFERENCES users(user_id), -- Foreign key relation to the users table
    FOREIGN KEY (ratee_id) REFERENCES participants(participant_id), -- Foreign key relation to the participants table
    FOREIGN KEY (space_id) REFERENCES spaces(space_id), -- Foreign key relation to the spaces table
    FOREIGN KEY (metric_id) REFERENCES metrics(metric_id), -- Foreign key relation to the metrics table
    UNIQUE (rater_id, ratee_id, space_id, metric_id) -- Unique constraint to enforce the combination
);
