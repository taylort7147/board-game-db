'use strict';

function GameList(props) {
    var games = props;
    var gameNodes = [];
    for (var key in games) {
        var g = games[key];
        gameNodes.push(
            <li className="bgdb-list-item" key={g.id}>
                <Game
                    key={g.id}
                    game={g}></Game>
            </li>
        );
    }
    return <ul>
        {gameNodes}
    </ul>;
}

function formatPlayTimeRangeString(min, max) {
    if (max === undefined) return "?";
    if (max < 30) return "<30";
    if (max <= 60) return "30-60";
    if (max <= 120) return "60-120";
    return ">120";
}

function formatPlayerRange(min, max) {
    if (min == max) return min.toString();
    return min.toString() + "-" + max;
}

function Game(props) {
    var g = props.game;
    var complexityClass = "bgdb-complexity-" + Math.min(5, Math.max(1,Math.floor(g.complexity)));
    return <div className="card bgdb-card mb-3">
        <div className="row bgdb-row g-0">
            <div className="col">
                <div className="card-body bgdb-card-body">
                    <h3 className="card-title">{g.title}</h3>
                    <small>{g.primaryMechanic.name}</small>
                </div>
            </div>
            <div className="col-auto bgdb-summary">
                <div className="row bgdb-row g-0">
                    <img src="/img/icon/meeple.png" className="bgdb-small-icon"></img>
                    <span>{formatPlayerRange(g.minimumPlayerCount, g.maximumPlayerCount)}</span>
                </div>
                <div className="row bgdb-row g-0">
                    <img src="/img/icon/clock.png" className="bgdb-small-icon"></img>
                    <span>{formatPlayTimeRangeString(g.minimumPlayTimeMinutes, g.maximumPlayTimeMinutes)}</span>
                </div>
                <div className="row bgdb-row g-0">
                    <img src="/img/icon/think.png" className="bgdb-small-icon"></img>
                    <span className={complexityClass}>{g.complexity}</span><span>/5</span>
                </div>
                <div className="row bgdb-row g-0">
                    <img src="/img/icon/location.png" className="bgdb-small-icon"></img>
                    <span>{g.location}</span>
                </div>
            </div>
            <div className="col-auto bgdb-image-container">
                <img src={g.pictureUrl} className="img-fluid rounded-start bgdb-thumbnail" alt="" />
            </div>
        </div>
    </div>;
}

export {
    Game,
    GameList as default
};