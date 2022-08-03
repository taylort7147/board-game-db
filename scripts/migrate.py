from os.path import isdir, isfile, join, abspath, dirname
import csv
from unicodedata import name

# For database
import pyodbc
import pandas as pd
from sqlalchemy.engine import URL, create_engine
import sqlite3


root = abspath(join(dirname(__file__), ".."))
data_file = join(root, "data", "board_game_data.csv")
connection_string = f"""{join(root, "data", "games.db")}"""


# Database classes
def get_name_attr(x):
    return x.name


class Game():
    def __init__(self):
        self.id = None
        self.title = ""
        self.location = ""
        self.primary_game_type = ""
        self.estimated_play_time_group = ""
        self.estimated_play_time_raw = 0
        self.min_players = 0
        self.max_players = 0,
        self.min_play_time = 0
        self.max_play_time = 0
        self.complexity_raw = 0.0
        self.mechanics = []
        self.play_styles = []
        self.categories = []

    def clean_mechanics(self, string):
        mechanics_set = set()
        mechanics_to_split = []

        special_mechanics = [
            "deck, bag, and pool building",
            "i cut, you choose",
            "deck, bag, and pool drafting"
        ]

        extra_mechanics = [
            # "dexterity",
            # "information_relay"
        ]

        substitutions = [
            ("set up", "setup"),
            ("rerolling", "re-rolling"),
            ("progression turn order", "progressive turn order"),
            ("pick up and deliver", "pick-up and deliver"),
            ("pickup and deliver", "pick-up and deliver"),
            ("increase value of unknown", "increase value of unchosen"),
            ("deck, bag, and pool drafting", "deck, bag, and pool building"),
            ("drafing", "drafting")
        ]

        # Normalize case
        mechanics = string.lower()

        # Make substitions
        for x, y in substitutions:
            mechanics = mechanics.replace(x, y)

        # Process and remove mechanics which may cause parsing issues later
        for mechanic in special_mechanics:
            if mechanic in mechanics:
                mechanics_set.add(mechanic)
                mechanics = mechanics.replace(mechanic, "")

        # Split semi-colon-separated lists first
        if ";" in mechanics:
            tempA, tempB = mechanics.split(";")
            # print(f"tempA: {tempA}, tempB: {tempB}")
            mechanics_to_split.append(tempA)
            mechanics_to_split.append(tempB)
        else:
            # print(f"mechanics: {mechanics}")
            mechanics_to_split.append(mechanics)

        # Split comma-separated lists
        for mechanic_to_split in mechanics_to_split:
            for mechanic in mechanic_to_split.split(","):
                mechanic = mechanic.lower()
                mechanic = mechanic.strip()
                if len(mechanic) > 0:
                    mechanics_set.add(mechanic)

        # # Debug printing
        # print("Mechanics:")
        # for mechanic in self.mechanics:
        #     print(f"    {mechanic}")
        # print()
        # print(f"{len(self.mechanics)} mechanics found.")

        mechanics_list = [Mechanic(mechanic) for mechanic in mechanics_set]
        mechanics_list.sort(key=get_name_attr)
        return mechanics_list

    def clean_and_set_mechanics(self, string):
        self.mechanics = self.clean_mechanics(string)

    def clean_and_set_play_styles(self, string):
        play_styles_set = set()

        # Normalize
        play_styles = string.lower()

        # Split comma-separated list
        play_styles_list = play_styles.split(",")

        # Clean
        for play_style in play_styles_list:
            play_style = play_style.strip()
            if len(play_style) > 0:
                play_styles_set.add(play_style)

        # Convert to list and sort
        self.play_styles = [PlayStyle(play_style)
                            for play_style in play_styles_set]
        self.play_styles.sort(key=get_name_attr)

    def clean_and_set_categories(self, string):
        self.categories = []
        categories_set = set()

        substitutions = [
            ("card name", "card game"),
            ("campaign game", "campaign"),
            ("app driven", "app-driven"),
            ("cooperative game", "cooperative"),
            ("device driven", "device-driven"),
            ("diseases", "disease"),
            ("economic", "economy"),
            ("education", "educational"),
            ("fairytales", "fairy tale"),
            ("industry/manufacturing", "industry,manufacturing"),
            ("kids game", "children's game"),
            ("kids", "children's game"),
            ("legacy,", "legacy game,"),
            ("memory,", "memory game,"),
            ("mission,", "missions,"),
            ("monster,", "monsters,"),
            ("movie,", "movies,"),
            ("role-playing game", "role playing"),
            ("scanarios", "scenario"),
            ("supervillans", "supervillains"),
            ("team-based game", "team-based"),
            ("teams-based game", "team-based"),
            ("train,", "trains,"),
            ("traitor game", "traitor")
        ]

        # Normalize
        categories = string.lower()

        # Make substitutions
        for x, y in substitutions:
            categories = categories.replace(x, y)

        # Split comma-separated list
        categories_list = categories.split(",")

        # Clean
        for category in categories_list:
            category = category.strip()
            if len(category) > 0:
                categories_set.add(category)

        # Convert to list and sort
        self.categories = [Category(category) for category in categories_set]
        self.categories.sort(key=get_name_attr)


class NameIdPair():
    def __init__(self, name):
        self.id = None
        self.name = name
    
    def __eq__(self, other):
        return self.id == other.id and self.name == other.name

    def __hash__(self):
        return hash(self.name)

class Mechanic(NameIdPair):
    pass


class PlayStyle(NameIdPair):
    pass


class Category(NameIdPair):
    pass

# Database functions


def get_mechanic_id(conn, mechanic):
    mechanic_result = pd.read_sql_query(
        f"SELECT [Id], [Name] FROM [Mechanic] WHERE [Name]=? LIMIT 1", conn, params=(mechanic.name,))
    if(not mechanic_result.empty):
        return int(mechanic_result["Id"].iloc[0])


def try_insert_mechanic(conn, mechanic):
    mechanic_id = get_mechanic_id(conn, mechanic)
    if mechanic_id is None:
        cursor = conn.cursor()
        result = cursor.execute(
            "INSERT INTO [Mechanic] ([Name]) VALUES (?) RETURNING [Id]", (mechanic.name,))
        mechanic_id = int(next(result)[0])
        # print(f"Inserted new mechanic '{mechanic.name}' with ID {mechanic_id}")
    return mechanic_id


def get_play_style_id(conn, play_style):
    play_style_result = pd.read_sql_query(
        f"SELECT [Id], [Name] FROM [PlayStyle] WHERE [Name]=? LIMIT 1", conn, params=(play_style.name,))
    if(not play_style_result.empty):
        return int(play_style_result["Id"].iloc[0])


def try_insert_play_style(conn, play_style):
    play_style_id = get_play_style_id(conn, play_style)
    if play_style_id is None:
        cursor = conn.cursor()
        result = cursor.execute(
            "INSERT INTO [PlayStyle] ([Name]) VALUES (?) RETURNING [Id]", (play_style.name,))
        play_style_id = int(next(result)[0])
        # print(
        #     f"Inserted new play style '{play_style.name}' with ID {play_style_id}")
    return play_style_id


def get_category_id(conn, category):
    category_result = pd.read_sql_query(
        f"SELECT [Id], [Name] FROM [Category] WHERE [Name]=? LIMIT 1", conn, params=(category.name,))
    if(not category_result.empty):
        return int(category_result["Id"].iloc[0])


def try_insert_category(conn, category):
    category_id = get_category_id(conn, category)
    if category_id is None:
        cursor = conn.cursor()
        result = cursor.execute(
            "INSERT INTO [Category] ([Name]) VALUES (?) RETURNING [Id]", (category.name,))
        category_id = int(next(result)[0])
        # print(f"Inserted new category '{category.name}' with ID {category_id}")
    return category_id


def get_game_id(conn, game):
    game_result = pd.read_sql_query(
        f"SELECT [Id], [Title] FROM [Game] WHERE [Title]=? LIMIT 1", conn, params=(game.title,))
    if(not game_result.empty):
        return int(game_result["Id"].iloc[0])


def try_insert_game(conn, game, mechanic_dict={}, play_style_dict={}, category_dict={}):
    game_id = get_game_id(conn, game)
    if game_id is None:
        cursor = conn.cursor()

        # print(f"Primary game type: {game.primary_game_type.name}")
        # print(mechanic_dict)
        primary_game_type = mechanic_dict.get(game.primary_game_type.name, None)

        # TODO: Add complexity
        result = cursor.execute(
            """
            INSERT INTO [Game] (
                [Title], 
                [Location], 
                [Complexity],
                [PrimaryMechanicId],
                [MinimumPlayTimeMinutes], 
                [MaximumPlayTimeMinutes], 
                [MinimumPlayerCount], 
                [MaximumPlayerCount])
            VALUES (?, ?, ?, ?, ?, ?, ?, ?) RETURNING [Id]
            """, (
                game.title,
                game.location,
                game.complexity_raw,
                primary_game_type.id,
                game.min_play_time,
                game.max_play_time,
                game.min_players,
                game.max_players))
        game_id = int(next(result)[0])
        print(f"Inserted new game '{game.title}' with ID {game_id}")

    for mechanic in game.mechanics:
        mechanic_id = try_insert_mechanic(conn, mechanic)
        print(f"Attempting to link game '{game.title} ({game_id})' to mechanic '{mechanic.name} ({mechanic_id})'")
        try_link_game_mechanic(conn, game_id, mechanic_id)
    
    for play_style in game.play_styles:
        play_style_id = try_insert_play_style(conn, play_style)
        print(f"Attempting to link game '{game.title} ({game_id})' to play style '{play_style.name} ({play_style_id})'")
        try_link_game_play_style(conn, game_id, play_style_id)

    for category in game.categories:
        category_id = try_insert_category(conn, category)
        print(f"Attempting to link game '{game.title} ({game_id})' to category '{category.name} ({category_id})'")
        try_link_game_category(conn, game_id, category_id)

    return game_id

def try_link_game_mechanic(conn, game_id, mechanic_id):
    cursor = conn.cursor()
    result = cursor.execute("""
        INSERT INTO [GameMechanic] (
            [GamesId],
            [MechanicsId])
        VALUES(?, ?)
    """, (game_id, mechanic_id))

def try_link_game_play_style(conn, game_id, play_style_id):
    cursor = conn.cursor()
    result = cursor.execute("""
        INSERT INTO [GamePlayStyle] (
            [GamesId],
            [PlayStylesId])
        VALUES(?, ?)
    """, (game_id, play_style_id))

def try_link_game_category(conn, game_id, category_id):
    cursor = conn.cursor()
    result = cursor.execute("""
        INSERT INTO [CategoryGame] (
            [GamesId],
            [CategoriesId])
        VALUES(?, ?)
    """, (game_id, category_id))


# Main
all_games = []
all_mechanics = set()
all_play_styles = set()
all_categories = set()

# Parse input file
with open(data_file, "r") as fp:
    reader = csv.reader(fp)
    header = next(reader)  # skip header
    # print(f"Header: {header}")

    for row in reader:
        game_title, location, primary_game_type, estimated_play_time, estimated_play_time_raw, min_players, max_players, complexity, complexity_raw, play_styles, mechanics, categories = row

        game = Game()
        game.title = game_title

        game.location = location
        game.primary_game_type = game.clean_mechanics(primary_game_type)[0]
        game.estimated_play_time_group = estimated_play_time
        game.estimated_play_time_raw = int(estimated_play_time_raw) if len(
            estimated_play_time_raw) > 0 else None
        game.min_play_time = game.estimated_play_time_raw or 0
        game.max_play_time = game.estimated_play_time_raw or game.min_play_time
        game.min_players = int(min_players)
        game.max_players = int(max_players)
        game.complexity_raw = float(complexity_raw)

        game.clean_and_set_mechanics(game.primary_game_type.name + "," + mechanics)
        game.clean_and_set_play_styles(play_styles)
        game.clean_and_set_categories(categories)

        # assert game.primary_game_type in game.mechanics, f"Primary game type ({game.primary_game_type}) not in mechanics list ({game.mechanics})."

        for mechanic in game.mechanics:
            all_mechanics.add(mechanic)
        for play_style in game.play_styles:
            all_play_styles.add(play_style)
        for category in game.categories:
            all_categories.add(category)

        all_games.append(game)

# # Debug printing
# all_mechanics = list(all_mechanics)
# all_mechanics.sort(key=lambda m: m.name)
# print(f"Mechanics ({len(all_mechanics)}):")
# for mechanic in all_mechanics:
    # print(f"    {mechanic.name}")
# print()

# all_play_styles = list(all_play_styles)
# all_play_styles.sort()
# print(f"Play styles ({len(all_play_styles)}):")
# for play_style in all_play_styles:
#     print(f"    {play_style}")
# print()

# all_categories = list(all_categories)
# all_categories.sort()
# print(f"Categories ({len(all_categories)}):")
# for category in all_categories:
#     print(f"    {category}")
# print()

# Add to database
# connection_url = URL.create("sqlite+pysqlite", query={"odbc_connect": connection_string})
# connection_url = f"sqlite:///{join(root, 'BoardGameDB', 'games.db')}"

# print(connection_string)
# print(connection_url)
# engine = create_engine(connection_url)
# conn = pyodbc.connect(connection_string)
# conn = engine.connect()

conn = sqlite3.Connection(connection_string)


db_games_by_title = {}
db_mechanics_by_name = {}
db_play_styles_by_name = {}
db_categories_by_name = {}

for mechanic in all_mechanics:
    mechanic_id = try_insert_mechanic(conn, mechanic)
    mechanic.id = mechanic_id
    db_mechanics_by_name[mechanic.name] = mechanic

for play_style in all_play_styles:
    play_style_id = try_insert_play_style(conn, play_style)
    play_style.id = play_style_id
    db_play_styles_by_name[play_style.name] = play_style

for category in all_categories:
    category_id = try_insert_category(conn, category)
    category.id = category_id
    db_categories_by_name[category.name] = category

for game in all_games:
    game_id = try_insert_game(conn, game,
                              mechanic_dict=db_mechanics_by_name,
                              play_style_dict=db_play_styles_by_name,
                              category_dict=db_categories_by_name)
    game.id = game_id
    db_games_by_title[game.title] = game



print(f"Finished processing {len(all_games)} games")

conn.commit()
