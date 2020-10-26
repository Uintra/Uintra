
export const EMOJI_CONFIG = {
  path: 'wwwroot/assets/emoji/',
  width: 20,
  height: 20,
  class: 'emoji-icon'
};

export interface IEmoji {
  src: string;
  title: string;
  shortcut: string;
  width: number;
  height: number;
  class: string;
}

export const emojiList: IEmoji[] = [
  {
    src: `${EMOJI_CONFIG.path}smile.svg`,
    title: 'Smile (:))',
    shortcut: ':)',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} smile`,
  },
  {
    src: `${EMOJI_CONFIG.path}sad.svg`,
    title: 'Sad (:()',
    shortcut: ':(',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} sad`,
  },
  {
    src: `${EMOJI_CONFIG.path}wink.svg`,
    title: 'Wink (;))',
    shortcut: ';)',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} wink`,
  },
  {
    src: `${EMOJI_CONFIG.path}shocked.svg`,
    title: 'Shocked (:|)',
    shortcut: ':|',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} shocked`,
  },
  {
    src: `${EMOJI_CONFIG.path}tease.svg`,
    title: 'Tease (:p)',
    shortcut: ':p',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} tease`,
  },
  {
    src: `${EMOJI_CONFIG.path}funny.svg`,
    title: 'Funny (:D)',
    shortcut: ':D',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} funny`,
  },
  {
    src: `${EMOJI_CONFIG.path}angry.svg`,
    title: 'Angry (:<)',
    shortcut: ':<',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} angry`,
  },
  {
    src: `${EMOJI_CONFIG.path}skeptical.svg`,
    title: 'Skeptical (:^))',
    shortcut: ':^',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} skeptical`,
  },
  {
    src: `${EMOJI_CONFIG.path}surprised.svg`,
    title: 'Surprised (:o)',
    shortcut: ':o',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} surprised`,
  },
  {
    src: `${EMOJI_CONFIG.path}great.svg`,
    title: 'Great (:+1)',
    shortcut: ':+1',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} great`,
  },
  {
    src: `${EMOJI_CONFIG.path}joy.svg`,
    title: 'Joy (:-))',
    shortcut: ':-)',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} joy`,
  },
  {
    src: `${EMOJI_CONFIG.path}love.svg`,
    title: 'Love (:x)',
    shortcut: ':x',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} love`,
  },
  {
    src: `${EMOJI_CONFIG.path}party.svg`,
    title: 'Party (<o))',
    shortcut: '<o',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} party`,
  },
  {
    src: `${EMOJI_CONFIG.path}fever.svg`,
    title: 'Fever (:fever)',
    shortcut: ':fever',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} fever`,
  },
  {
    src: `${EMOJI_CONFIG.path}sleepy.svg`,
    title: 'Sleepy (|-))',
    shortcut: '|-)',
    width: EMOJI_CONFIG.width,
    height: EMOJI_CONFIG.height,
    class: `${EMOJI_CONFIG.class} sleepy`
  },
];
